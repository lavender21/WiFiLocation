package com.example.lavender.wifilocation;

import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
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
    public static Button button, btnStartWifi,btnStopWifi;
    private static TextView showRssi;
    private BroadcastReceiver receiver = new BroadcastReceiver() {
        @Override
        public void onReceive(Context context, Intent intent) {
            ArrayList<String> data = intent.getStringArrayListExtra("wifiList");
            showRssi.setText(data.toString());
        }
    };
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        init();

    }



    public void init(){
        button = (Button) findViewById(R.id.button);
        button.setOnClickListener(this);
        btnStartWifi = (Button)findViewById(R.id.btnStartWifi);
        btnStartWifi.setOnClickListener(this);
        btnStopWifi = (Button)findViewById(R.id.btnStopWifi);
        btnStopWifi.setOnClickListener(this);
        showRssi = (TextView)findViewById(R.id.textView);
    }

    @Override
    public void onClick(View v) {
        Intent intent = new Intent(MainActivity.this, GetRSSIService.class);
        switch (v.getId()){
            // 测试http链接
            case R.id.button:
                httpRequest();
                break;
            // 开启获取wifi信号强度
            case R.id.btnStartWifi:
                startService(intent);
                break;
            // 关闭获取wifi信号强度
            case R.id.btnStopWifi:
                stopService(intent);
                break;
        }
    }

    // http请求;
    public void httpRequest()
    {
        JSONObject jsonObject = new JSONObject();
        try {
            jsonObject.put("lastname","zaizai");
        }catch (JSONException e){
            e.printStackTrace();
        }
        HttpConnect httpConnect = new HttpConnect();
        httpConnect.execute("GET", HttpConnect.APITEST, jsonObject.toString());
    }
}






































