package com.thomaskuenneth.stopwatch_aac;

import android.arch.lifecycle.Lifecycle;
import android.arch.lifecycle.LifecycleObserver;
import android.arch.lifecycle.OnLifecycleEvent;
import android.os.Handler;

import java.util.Timer;
import java.util.TimerTask;

public class StopwatchLifecycleObserver implements LifecycleObserver {

    private final StopwatchViewModel model;
    private final Handler handler;

    private Timer timer;
    private TimerTask timerTask;

    StopwatchLifecycleObserver(StopwatchViewModel model, Handler handler) {
        this.model = model;
        this.handler = handler;
    }

    @OnLifecycleEvent(Lifecycle.Event.ON_RESUME)
    public void startTimer() {
        timer = new Timer();
        boolean running = StopwatchViewModel.getBoolean(model.getIsRunning().getValue());
        if (running) {
            scheduleAtFixedRate();
        }
    }

    @OnLifecycleEvent(Lifecycle.Event.ON_PAUSE)
    public void stopTimer() {
        timer.cancel();
    }

    void stop() {
        timerTask.cancel();
    }

    void scheduleAtFixedRate() {
        model.getStarted().setValue(System.currentTimeMillis()
                - StopwatchViewModel.getLong(model.getDiff().getValue()));
        timerTask = new TimerTask() {
            @Override
            public void run() {
                handler.post(()
                        -> model.getDiff().setValue(System.currentTimeMillis()
                        - StopwatchViewModel.getLong(model.getStarted().getValue())));
            }
        };
        timer.scheduleAtFixedRate(timerTask, 0, 200);
    }
}
