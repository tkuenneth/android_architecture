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
        Boolean running = model.running.getValue();
        if ((running != null) && (running)) {
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
        Long diff = model.diff.getValue();
        if (diff != null) {
            model.started.setValue(System.currentTimeMillis() - diff);
        }
        timerTask = new TimerTask() {
            @Override
            public void run() {
                Long started = model.started.getValue();
                if (started != null) {
                    handler.post(()
                            -> model.diff.setValue(System.currentTimeMillis() - started));
                }
            }
        };
        timer.scheduleAtFixedRate(timerTask, 0, 200);
    }
}
