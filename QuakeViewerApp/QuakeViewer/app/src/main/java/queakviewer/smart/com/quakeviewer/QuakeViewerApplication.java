package queakviewer.smart.com.quakeviewer;

import android.app.Application;

import com.baidu.mapapi.SDKInitializer;

import queakviewer.smart.com.quakeviewer.service.LocationService;

/**
 * Created by Ares on 2017/4/20.
 */

public class QuakeViewerApplication extends Application {

    public LocationService locationService;

    @Override
    public void onCreate() {

        super.onCreate();
        locationService = new LocationService(getApplicationContext());
        SDKInitializer.initialize(getApplicationContext());
    }

    @Override
    public void onTerminate() {

        super.onTerminate();

    }
}
