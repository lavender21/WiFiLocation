package com.example.lavender.wifilocation;

import android.app.Service;
import android.content.Intent;
import android.net.wifi.WifiInfo;
import android.os.Build;
import android.os.Handler;
import android.os.IBinder;
import android.net.wifi.ScanResult;
import android.net.wifi.WifiManager;
import android.os.Message;
import android.util.Log;
import android.widget.Toast;

import java.util.ArrayList;
import java.util.List;
import java.util.Timer;
import java.util.TimerTask;

/*这个服务用来获取wifi信号列表，并对信号进行滤波，聚类进行优化处理。最终得到一组准确度高的信号强度列表。*/

public class GetRSSIService extends Service {
    private WifiManager wifiManager;
    private Timer timer;
    @Override
    public void onCreate() {
        super.onCreate();
        MainActivity.btnStartWifi.setEnabled(false);
        Toast.makeText(this,"开始扫描信号强度",Toast.LENGTH_SHORT).show();
    }

    @Override
    public int onStartCommand(Intent intent, int flags, int startId) {
        startTimer();
        return super.onStartCommand(intent, flags, startId);
    }

    @Override
    public void onDestroy() {
        super.onDestroy();
        stopTimer();
        MainActivity.btnStartWifi.setEnabled(true);
        Toast.makeText(this,"wifi信号强度扫描结束",Toast.LENGTH_SHORT).show();
    }

    public GetRSSIService() {
    }

    @Override
    public IBinder onBind(Intent intent) {
        // TODO: Return the communication channel to the service.
        throw new UnsupportedOperationException("Not yet implemented");
    }

    // 开启计时器
    private void startTimer() {
        timer = new Timer();
        TimerTask task = new TimerTask() {
            @Override
            public void run() {
                getWifiRssi();
            }
        };
        timer.schedule(task,1000,3000);
    }

    // 停止计时器
    private void stopTimer(){
        timer.cancel();
    }
    // 获取wifi信号强度
    private void getWifiRssi(){
        //打开wifi
        wifiManager = (WifiManager) getSystemService(WIFI_SERVICE);
        if (!wifiManager.isWifiEnabled()) {
            wifiManager.setWifiEnabled(true);
        }
        Log.i("WIFI","get wifiManager");
        // 获取到周围的wifi列表
        List<ScanResult> scanResults = wifiManager.getScanResults();
        Log.i("WIFI","get scanResults");
        ArrayList<String>  wifiList = new ArrayList<String>();
        for (ScanResult scanResult : scanResults){
            String str = "{\"name\":"+scanResult.SSID+
                    "\"mac\":"+scanResult.BSSID+",\"rssi\":"+scanResult.level+"}";
            wifiList.add(str);
        }
        Intent intent = new Intent();
        intent.setAction("wifiData");
        intent.putStringArrayListExtra("wifiList",wifiList);
        intent.putExtra("mobileModel", Build.MODEL);
        sendBroadcast(intent);
        Log.i("WIFI","get intent");
}

}
