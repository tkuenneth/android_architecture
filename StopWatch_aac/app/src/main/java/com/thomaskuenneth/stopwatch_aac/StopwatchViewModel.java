package com.thomaskuenneth.stopwatch_aac;

import android.arch.lifecycle.MutableLiveData;
import android.arch.lifecycle.ViewModel;

class StopwatchViewModel extends ViewModel {

    private final MutableLiveData<Boolean> isRunning = new MutableLiveData<>();
    private final MutableLiveData<Long> diff = new MutableLiveData<>();
    private final MutableLiveData<Long> started = new MutableLiveData<>();

    MutableLiveData<Boolean> getIsRunning() {
        return isRunning;
    }

    MutableLiveData<Long> getDiff() {
        return diff;
    }

    MutableLiveData<Long> getStarted() {
        return started;
    }

    static boolean getBoolean(Boolean data) {
        if (data != null) {
            return data;
        }
        return false;
    }

    static long getLong(Long data) {
        if (data != null) {
            return data;
        }
        return 0L;
    }
}
