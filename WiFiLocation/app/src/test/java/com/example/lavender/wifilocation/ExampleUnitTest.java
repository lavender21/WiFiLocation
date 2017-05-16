package com.example.lavender.wifilocation;

import org.junit.Test;

import static org.junit.Assert.*;

/**
 * Example local unit test, which will execute on the development machine (host).
 *
 * @see <a href="http://d.android.com/tools/testing">Testing documentation</a>
 */
public class ExampleUnitTest {
    @Test
    public void addition_isCorrect() throws Exception {
        assertEquals(4, 2 + 2);
    }
    @Test
    public void calculate_distance_isCorrect() throws Exception {
        GetStepLengthActivity getStepLengthActivity = new GetStepLengthActivity();
        double lat1 = 34.253472,lnt1 = 108.986201,lat2 = 34.25336,lnt2 = 108.986569;
        double distance = getStepLengthActivity.GetDistance(lat1,lnt1,lat2,lnt2);

        assertEquals("",distance,3608,0);
    }
}