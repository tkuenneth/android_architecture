package com.thomaskuenneth.stopwatch;

import android.app.Activity;
import android.os.Bundle;
import android.widget.Button;
import android.widget.TextView;

import java.text.DateFormat;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.Locale;
import java.util.TimeZone;
import java.util.Timer;
import java.util.TimerTask;

public class StopwatchActivity extends Activity {

    private static final String KEY_DIFF = "diff";
    private static final String KEY_RUNNING = "running";

    private final DateFormat format;
    private Timer timer;
    private TimerTask timerTask;
    private TextView time;
    private Button startStop;
    private Button reset;
    private long started;
    private boolean isRunning;
    private long diff;

    public StopwatchActivity() {
        format = new SimpleDateFormat("HH:mm:ss:SSS",
                Locale.US);
        format.setTimeZone(TimeZone.getTimeZone("UTC"));
    }

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        time = findViewById(R.id.time);
        startStop = findViewById(R.id.start_stop);
        startStop.setOnClickListener(v -> {
            isRunning = !isRunning;
            if (isRunning) {
                scheduleAtFixedRate();
            } else {
                timerTask.cancel();
            }
            updateUI();
        });
        reset = findViewById(R.id.reset);
        reset.setOnClickListener(v -> clearTime());
        if (savedInstanceState != null) {
            getValuesFromBundle(savedInstanceState);
            setTime();
        } else {
            isRunning = false;
            clearTime();
        }
    }

    @Override
    protected void onSaveInstanceState(Bundle outState) {
        super.onSaveInstanceState(outState);
        outState.putLong(KEY_DIFF, diff);
        outState.putBoolean(KEY_RUNNING, isRunning);
    }

    @Override
    protected void onRestoreInstanceState(Bundle savedInstanceState) {
        super.onRestoreInstanceState(savedInstanceState);
        getValuesFromBundle(savedInstanceState);
    }

    @Override
    protected void onResume() {
        super.onResume();
        timer = new Timer();
        updateUI();
        if (isRunning) {
            scheduleAtFixedRate();
        }
    }

    @Override
    protected void onPause() {
        super.onPause();
        timer.cancel();
    }

    private void clearTime() {
        time.setText(R.string.cleared);
        diff = 0;
    }

    private void updateUI() {
        startStop.setText(isRunning ? R.string.stop : R.string.start);
        reset.setEnabled(!isRunning);
    }

    private void getValuesFromBundle(Bundle b) {
        diff = b.getLong(KEY_DIFF);
        isRunning = b.getBoolean(KEY_RUNNING);
    }

    private void setTime() {
        time.setText(format.format(new Date(diff)));
    }

    private void scheduleAtFixedRate() {
        started = System.currentTimeMillis() - diff;
        timerTask = new TimerTask() {
            @Override
            public void run() {
                diff = System.currentTimeMillis() - started;
                runOnUiThread(() -> setTime());
            }
        };
        timer.scheduleAtFixedRate(timerTask, 0, 200);
    }
}
