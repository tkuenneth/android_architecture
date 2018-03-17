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
        boolean running = model.isRunning();
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
        model.started.setValue(System.currentTimeMillis() - model.getDiff());
        timerTask = new TimerTask() {
            @Override
            public void run() {
                handler.post(()
                        -> model.diff.setValue(System.currentTimeMillis() - model.getStarted()));
            }
        };
        timer.scheduleAtFixedRate(timerTask, 0, 200);
    }
}
