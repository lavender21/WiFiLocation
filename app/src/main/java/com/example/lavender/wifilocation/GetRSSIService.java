package com.example.lavender.wifilocation;

import android.app.Service;
import android.content.Intent;
import android.net.wifi.WifiInfo;
import android.os.Handler;
import android.os.IBinder;
import android.net.wifi.ScanResult;
import android.net.wifi.WifiManager;
import android.os.Message;
import android.util.Log;
import android.widget.Toast;

import com.google.gson.Gson;
import com.google.gson.reflect.TypeToken;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.lang.reflect.Type;
import java.util.ArrayList;
import java.util.List;
import java.util.Timer;
import java.util.TimerTask;


public class GetRSSIService extends Service {
    private WifiManager wifiManager;
    private WifiInfo wifinfo;
    private Timer timer;
    @Override
    public void onCreate() {
        super.onCreate();
        MainActivity.btnStartWifi.setEnabled(false);
        getWifiRssi();
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
        timer.schedule(task,1000,5000);
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
        wifinfo = wifiManager.getConnectionInfo();
        // 获取到周围的wifi列表
        List<ScanResult> scanResults = wifiManager.getScanResults();
        Log.i("WIFI","get scanResults");
        // 用JSON存储
       /* JSONArray wifiList = new JSONArray();
        for (ScanResult scanResult : scanResults){
            JSONObject json = new JSONObject();
            try{
                json.put("name",scanResult.SSID);
                json.put("rssi",scanResult.level);
                Log.i("WIFI","get SSID　level");
            }catch (JSONException e){
                e.printStackTrace();
            }
            wifiList.put(json);
        }*/
        ArrayList<String>  wifiList = new ArrayList<String>();
        for (ScanResult scanResult : scanResults){
            String str = "{\"name\":"+scanResult.SSID+",\"rssi\":"+scanResult.level+"}";
            wifiList.add(str);
        }

        Intent intent = new Intent();
        intent.setAction("wifiData");
        intent.putStringArrayListExtra("wifiList",wifiList);
        Log.i("WIFI","get intent");
}
    public class WifiInfomation{
        private String SSID;
        private String rssi;
    }

}
