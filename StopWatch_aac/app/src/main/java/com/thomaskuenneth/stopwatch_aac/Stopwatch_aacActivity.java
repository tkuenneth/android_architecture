package com.thomaskuenneth.stopwatch_aac;

import android.arch.lifecycle.Lifecycle;
import android.arch.lifecycle.LifecycleObserver;
import android.arch.lifecycle.OnLifecycleEvent;
import android.arch.lifecycle.ViewModelProviders;
import android.os.Bundle;
import android.support.v7.app.AppCompatActivity;
import android.widget.Button;
import android.widget.TextView;

import java.text.DateFormat;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.Locale;
import java.util.Timer;
import java.util.TimerTask;

public class Stopwatch_aacActivity extends AppCompatActivity
        implements LifecycleObserver {

    private static final DateFormat F = new SimpleDateFormat("HH:mm:ss:SSS",
            Locale.US);

    private StopwatchViewModel model;
    private Timer timer;
    private TimerTask timerTask;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        TextView time = findViewById(R.id.time);
        Button startStop = findViewById(R.id.start_stop);
        Button reset = findViewById(R.id.reset);
        model = ViewModelProviders.of(this).get(StopwatchViewModel.class);
        model.isRunning.observe(this, isRunning -> {
            final boolean running = model.isRunning();
            startStop.setText(running ? R.string.stop : R.string.start);
            reset.setEnabled(!running);
        });
        model.diff.observe(this, diff
                -> time.setText(F.format(new Date(model.getDiff()))));
        startStop.setOnClickListener(v ->
        {
            boolean running = !model.isRunning();
            model.isRunning.setValue(running);
            if (running) {
                scheduleAtFixedRate();
            } else {
                timerTask.cancel();
            }
        });
        reset.setOnClickListener(v -> model.diff.setValue(0L));
        getLifecycle().addObserver(this);
    }

    @OnLifecycleEvent(Lifecycle.Event.ON_RESUME)
    public void startTimer() {
        timer = new Timer();
        boolean running = model.isRunning();
        if (running) {
            scheduleAtFixedRate();
        }
    }

    @OnLifecycleEvent(Lifecycle.Event.ON_PAUSE)
    public void stopTimer() {
        timer.cancel();
    }

    private void scheduleAtFixedRate() {
        model.started.setValue(System.currentTimeMillis() - model.getDiff());
        timerTask = new TimerTask() {
            @Override
            public void run() {
                runOnUiThread(()
                        -> model.diff.setValue(System.currentTimeMillis() - model.getStarted()));
            }
        };
        timer.scheduleAtFixedRate(timerTask, 0, 200);
    }
}
