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

public class Stopwatch_aacActivity extends AppCompatActivity {

    private static final DateFormat F = new SimpleDateFormat("HH:mm:ss:SSS",
            Locale.US);

    private StopwatchViewModel model;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        TextView time = findViewById(R.id.time);
        Button startStop = findViewById(R.id.start_stop);
        Button reset = findViewById(R.id.reset);
        model = ViewModelProviders.of(this).get(StopwatchViewModel.class);
        StopwatchLifecycleObserver observer = new StopwatchLifecycleObserver(model, new Handler());
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
                observer.scheduleAtFixedRate();
            } else {
                observer.stop();
            }
        });
        reset.setOnClickListener(v -> model.diff.setValue(0L));
        getLifecycle().addObserver(observer);
    }
}
