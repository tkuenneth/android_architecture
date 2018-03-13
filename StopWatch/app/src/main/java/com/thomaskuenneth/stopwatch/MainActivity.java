package com.thomaskuenneth.stopwatch;

import android.app.Activity;
import android.os.Bundle;
import android.widget.Button;
import android.widget.TextView;

import java.text.DateFormat;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.Locale;
import java.util.Timer;
import java.util.TimerTask;

public class MainActivity extends Activity {

    private static final DateFormat F = new SimpleDateFormat("HH:mm:ss:SSS",
            Locale.US);
    private static final String KEY_DIFF = "diff";
    private static final String KEY_RUNNING = "running";

    private Timer timer;
    private TimerTask timerTask;
    private TextView time;
    private Button startStop;
    private Button reset;

    private boolean isRunning;
    private long started;
    private long diff;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        time = findViewById(R.id.time);
        startStop = findViewById(R.id.start_stop);
        startStop.setOnClickListener(v -> {
            isRunning = !isRunning;
            if (isRunning) {
                run();
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
            run();
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
        if (isRunning) {
            startStop.setText(R.string.stop);
        } else {
            startStop.setText(R.string.start);
        }
        reset.setEnabled(!isRunning);
    }

    private void getValuesFromBundle(Bundle b) {
        diff = b.getLong(KEY_DIFF);
        isRunning = b.getBoolean(KEY_RUNNING);
    }

    private void setTime() {
        time.setText(F.format(new Date(diff)));
    }

    private void run() {
        started = System.currentTimeMillis() - diff;
        timerTask = new TimerTask() {
            @Override
            public void run() {
                diff = System.currentTimeMillis() - started;
                setTime();
            }
        };
        timer.scheduleAtFixedRate(timerTask, 200, 200);
    }
}
