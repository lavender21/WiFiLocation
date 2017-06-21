package com.example.lavender.wifilocation;

import android.content.Context;
import android.graphics.Canvas;
import android.graphics.Color;
import android.graphics.Paint;
import android.graphics.RectF;
import android.view.View;

/**
 * Created by lavender on 2017/6/2.
 */


public class DrawView extends View {

    public DrawView(Context context) {
        super(context);
    }

    @Override
    protected void onDraw(Canvas canvas) {
        super.onDraw(canvas);

        Paint paint = new Paint();
        paint.setColor(Color.rgb(230,230,250));
        paint.setStrokeWidth(2f);

        canvas.drawRect(100,100,800,800,paint);

        paint.setColor(Color.RED);
        paint.setStyle(Paint.Style.FILL);
        paint.setStrokeWidth(10f);
        canvas.drawPoint(160, 390, paint);//画一个点
    }
}