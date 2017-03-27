package com.example.lavender.wifilocation;

import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.content.IntentFilter;
import android.os.AsyncTask;
import android.os.Handler;
import android.os.Message;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.Button;
import android.widget.TextView;

import org.json.JSONException;
import org.json.JSONObject;

import java.util.ArrayList;


public class MainActivity extends AppCompatActivity implements View.OnClickListener{
    public static Button btnStartWifi,btnStopWifi,btnStartCoord,btnStopCoord,btnGet,btnPost;
    public static TextView showRssi;
    private BroadcastReceiver receiver = new BroadcastReceiver() {
        @Override
        public void onReceive(Context context, Intent intent) {
            ArrayList<String> wifidata = intent.getStringArrayListExtra("wifiList");
            String model = intent.getStringExtra("mobleModel");
            String txt = "wifi信号强度列表：\n"+wifidata;
            txt += "\n当前手机型号:"+model;
            String sensordata = intent.getStringExtra("accValue");
            String sensordata2 = intent.getStringExtra("magnValue");
            txt +="\n\n当前传感器数据：\n"+sensordata+"\n"+sensordata2;
            showRssi.setText(txt);
        }
    };
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        init();
        // 注册对象，并添加接收数据的action;
        registerReceiver(receiver, new IntentFilter("wifiData"));
        registerReceiver(receiver, new IntentFilter("sensorData"));
    }



    public void init(){
        btnGet = (Button) findViewById(R.id.btnGet);
        btnGet.setOnClickListener(this);
        btnPost = (Button)findViewById(R.id.btnPost);
        btnPost.setOnClickListener(this);
        btnStartWifi = (Button)findViewById(R.id.btnStartWifi);
        btnStartWifi.setOnClickListener(this);
        btnStopWifi = (Button)findViewById(R.id.btnStopWifi);
        btnStopWifi.setOnClickListener(this);
        showRssi = (TextView)findViewById(R.id.textView);
        btnStartCoord = (Button)findViewById(R.id.btnStartCoord);
        btnStartCoord.setOnClickListener(this);
        btnStopCoord = (Button)findViewById(R.id.btnStopCoord);
        btnStopCoord.setOnClickListener(this);
    }

    @Override
    public void onClick(View v) {
        Intent intentWifi = new Intent(MainActivity.this, GetRSSIService.class);
//        Intent intentSensor = new Intent(MainActivity.this,GetCoordService.class);
        switch (v.getId()){
            // 测试http链接
            case R.id.btnGet:
                httpRequestGet();
                break;
            // 测试http连接
            case R.id.btnPost:
                httpRequestPost();
                break;
            // 开启获取wifi信号强度
            case R.id.btnStartWifi:
                startService(intentWifi);
                break;
            // 关闭获取wifi信号强度
            case R.id.btnStopWifi:
                stopService(intentWifi);
                break;
            // 进入传感器测试
            case R.id.btnStartCoord:
                  Intent intentSensor = new Intent(MainActivity.this, SensorTestActivity.class);
                  startActivity(intentSensor);
//                startService(intentSensor);
                break;
            // 关闭传感器
            case R.id.btnStopCoord:
//                stopService(intentSensor);
                break;
        }
    }

    // http请求;
    public void httpRequestGet()
    {
        HttpConnect httpConnect = new HttpConnect();
        httpConnect.execute("GET",HttpConnect.APITEST,"");
    }
    public void httpRequestPost()
    {
        JSONObject jsonObject = new JSONObject();
        try{
            jsonObject.put("coord","[12,34],[23,12],[24,12]");
            jsonObject.put("memory","android app 发送 http post请求测试");
            jsonObject.put("flag","0");
        }catch (JSONException e){
            e.printStackTrace();;
        }
        HttpConnect httpConnect = new HttpConnect();
        httpConnect.execute("POST", httpConnect.APIPOSTTEST,jsonObject.toString());
    }
}






































