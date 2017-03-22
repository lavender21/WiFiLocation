package com.example.lavender.wifilocation;

import android.app.Service;
import android.content.Intent;
import android.os.IBinder;

public class GetCoordService extends Service {
    public GetCoordService() {
    }

    @Override
    public IBinder onBind(Intent intent) {
        // TODO: Return the communication channel to the service.
        throw new UnsupportedOperationException("Not yet implemented");
    }
}
