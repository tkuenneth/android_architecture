package com.thomaskuenneth.stopwatch_aac;

import android.arch.lifecycle.ViewModelProviders;
import android.os.Bundle;
import android.os.Handler;
import android.support.v7.app.AppCompatActivity;
import android.widget.Button;
import android.widget.TextView;

import java.text.DateFormat;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.Locale;
import java.util.TimeZone;

public class Stopwatch_aacActivity extends AppCompatActivity {

    private final DateFormat F;

    private StopwatchViewModel model;

    public Stopwatch_aacActivity() {
        F = new SimpleDateFormat("HH:mm:ss:SSS",
                Locale.US);
        F.setTimeZone(TimeZone.getTimeZone(("UTC")));
    }

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        TextView time = findViewById(R.id.time);
        Button startStop = findViewById(R.id.start_stop);
        Button reset = findViewById(R.id.reset);
        model = ViewModelProviders.of(this).get(StopwatchViewModel.class);
        StopwatchLifecycleObserver observer = new StopwatchLifecycleObserver(model, new Handler());
        model.getIsRunning().observe(this, isRunning -> {
            final boolean running = StopwatchViewModel.getBoolean(isRunning);
            startStop.setText(running ? R.string.stop : R.string.start);
            reset.setEnabled(!running);
        });
        model.getDiff().observe(this, diff
                -> time.setText(F.format(new Date(StopwatchViewModel.getLong(diff)))));
        startStop.setOnClickListener(v ->
        {
            boolean running = !StopwatchViewModel.getBoolean(model.getIsRunning().getValue());
            model.getIsRunning().setValue(running);
            if (running) {
                observer.scheduleAtFixedRate();
            } else {
                observer.stop();
            }
        });
        reset.setOnClickListener(v -> model.getDiff().setValue(0L));
        getLifecycle().addObserver(observer);
    }
}
