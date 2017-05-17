package com.example.lavender.wifilocation;

import android.content.SharedPreferences;

import org.json.JSONException;
import org.json.JSONObject;

import java.text.DateFormat;
import java.util.Date;

import static android.content.Context.MODE_PRIVATE;

/**
 * Created by lavender on 2017/3/29.
 */

public class Common {

    // 获取手机型号
    public static int getMobileModel()
    {
        int num = 3;
        return num;
    }

    // 获取当前时间
    public static String getNowTime()
    {
        Date now = new Date();
        DateFormat df = DateFormat.getDateTimeInstance();
        String nowtime = df.format(now);
        return nowtime;
    }

    // 将String转换为Json
    public static JSONObject toJson(String str)
    {
        try{
            JSONObject json = new JSONObject(str);
            return json;
        }catch (JSONException e)
        {
            e.printStackTrace();
            return null;
        }
    }


}
