package com.thomaskuenneth.stopwatch_aac;

import android.arch.lifecycle.MutableLiveData;
import android.arch.lifecycle.ViewModel;

class StopwatchViewModel extends ViewModel {

    final MutableLiveData<Boolean> isRunning = new MutableLiveData<>();
    final MutableLiveData<Long> diff = new MutableLiveData<>();
    final MutableLiveData<Long> started = new MutableLiveData<>();

    boolean isRunning() {
        if (isRunning.getValue() != null) {
            return isRunning.getValue();
        }
        return false;
    }

    long getDiff() {
        if (diff.getValue() != null) {
            return diff.getValue();
        }
        return 0L;
    }

    long getStarted() {
        if (started.getValue() != null) {
            return started.getValue();
        }
        return 0L;
    }
}
