package com.thomaskuenneth.stopwatch_aac;

import android.arch.lifecycle.MutableLiveData;
import android.arch.lifecycle.ViewModel;

class StopwatchViewModel extends ViewModel {

    public final MutableLiveData<Boolean> running = new MutableLiveData<>();
    public final MutableLiveData<Long> diff = new MutableLiveData<>();
    public final MutableLiveData<Long> started = new MutableLiveData<>();

    public StopwatchViewModel() {
        running.setValue(false);
        diff.setValue(0L);
        started.setValue(0L);
    }
}
