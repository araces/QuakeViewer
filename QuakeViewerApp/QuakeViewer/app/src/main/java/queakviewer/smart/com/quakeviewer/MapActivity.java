package queakviewer.smart.com.quakeviewer;

import android.Manifest;
import android.annotation.TargetApi;
import android.app.Activity;
import android.content.Intent;
import android.content.pm.PackageManager;
import android.os.Build;
import android.os.Bundle;
import android.os.Handler;
import android.support.v7.app.AppCompatActivity;
import android.text.TextUtils;
import android.util.Log;
import android.view.View;
import android.widget.ProgressBar;
import android.widget.TextView;
import android.widget.Toast;

import com.baidu.location.BDLocation;
import com.baidu.location.BDLocationListener;
import com.baidu.location.LocationClient;
import com.baidu.location.LocationClientOption;
import com.baidu.location.Poi;
import com.baidu.mapapi.SDKInitializer;
import com.baidu.mapapi.cloud.CloudListener;
import com.baidu.mapapi.cloud.CloudManager;
import com.baidu.mapapi.cloud.CloudRgcInfo;
import com.baidu.mapapi.cloud.CloudRgcResult;
import com.baidu.mapapi.cloud.CloudSearchResult;
import com.baidu.mapapi.cloud.DetailSearchResult;
import com.baidu.mapapi.map.BaiduMap;
import com.baidu.mapapi.map.BitmapDescriptor;
import com.baidu.mapapi.map.BitmapDescriptorFactory;
import com.baidu.mapapi.map.MapPoi;
import com.baidu.mapapi.map.MapStatus;
import com.baidu.mapapi.map.MapStatusUpdate;
import com.baidu.mapapi.map.MapStatusUpdateFactory;
import com.baidu.mapapi.map.MarkerOptions;
import com.baidu.mapapi.map.MyLocationConfiguration;
import com.baidu.mapapi.map.MyLocationData;
import com.baidu.mapapi.map.OverlayOptions;
import com.baidu.mapapi.model.LatLng;

import org.json.JSONException;
import org.json.JSONObject;
import org.json.JSONTokener;

import java.io.IOException;
import java.util.ArrayList;
import java.util.List;

import butterknife.BindView;
import butterknife.ButterKnife;
import butterknife.OnClick;
import queakviewer.smart.com.quakeviewer.Utils.OnFinishedCallBack;
import queakviewer.smart.com.quakeviewer.Utils.Utils;
import queakviewer.smart.com.quakeviewer.Utils.WebClient;
import queakviewer.smart.com.quakeviewer.service.LocationService;

/**
 * Created by Ares on 2017/4/18.
 */

public class MapActivity extends Activity {

    public static final String Id = "MapActivity";

    @BindView(R.id.bmapView)
    com.baidu.mapapi.map.MapView mBaiduMap;

    @BindView(R.id.progressBar)
    ProgressBar progressBar;

    @BindView(R.id.address_in_detail)
    TextView addressInDetail;

    private static final int PERMISSION_STATUS = 100;

    private BaiduMap baiduMap;

    private String permissionInfo;

    LocationService locationService;

    public LocationClient mLocationClient = null;
    public BDLocationListener myListener = new MyLocationListener();

    private BitmapDescriptor mCurrentMarker;

    Handler handler;



    String province = "";
    String city="";
    String district = "";
    String street = "";
    String street_number = "";
    String addr = "";

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        SDKInitializer.initialize(getApplicationContext());

        //声明LocationClient类
        mLocationClient = new LocationClient(getApplicationContext());
        initLocation();
        //注册监听函数
        mLocationClient.registerLocationListener(myListener);

        setContentView(R.layout.dialog_map);

        ButterKnife.bind(this);

        baiduMap = mBaiduMap.getMap();

        baiduMap.setMapType(BaiduMap.MAP_TYPE_NORMAL);

        mCurrentMarker = BitmapDescriptorFactory
                .fromResource(R.drawable.icon_marka);

        Log.d(Id, "start service");


        selfRequestPermission();

        baiduMap.setOnMapStatusChangeListener(new BaiduMap.OnMapStatusChangeListener() {
            @Override
            public void onMapStatusChangeStart(MapStatus mapStatus) {

            }

            @Override
            public void onMapStatusChange(MapStatus mapStatus) {

            }

            @Override
            public void onMapStatusChangeFinish(final MapStatus mapStatus) {

                // 构造定位数据
                MyLocationData locData = new MyLocationData.Builder()
                        // 此处设置开发者获取到的方向信息，顺时针0-360
                        .latitude(mapStatus.target.latitude)
                        .longitude(mapStatus.target.longitude).build();
                // 设置定位数据
                baiduMap.setMyLocationData(locData);
                // 设置定位图层的配置（定位模式，是否允许方向信息，用户自定义定位图标）

                MyLocationConfiguration config = new MyLocationConfiguration(MyLocationConfiguration.LocationMode.NORMAL, true, mCurrentMarker);
                baiduMap.setMyLocationConfiguration(config);

                new Thread(new Runnable() {
                    @Override
                    public void run() {
                        GetLocationName(mapStatus.target);
                    }
                }).start();
            }
        });

        progressBar.setVisibility(View.VISIBLE);
        addressInDetail.setVisibility(View.GONE);

        baiduMap.setOnMapClickListener(listener);

        handler = new Handler();

    }

    private void initLocation() {
        LocationClientOption option = new LocationClientOption();
        option.setLocationMode(LocationClientOption.LocationMode.Hight_Accuracy);
        //可选，默认高精度，设置定位模式，高精度，低功耗，仅设备

        option.setCoorType("bd09ll");
        //可选，默认gcj02，设置返回的定位结果坐标系

        int span = 1000;
        option.setScanSpan(span);
        //可选，默认0，即仅定位一次，设置发起定位请求的间隔需要大于等于1000ms才是有效的

        option.setIsNeedAddress(true);
        //可选，设置是否需要地址信息，默认不需要

        option.setOpenGps(true);
        //可选，默认false,设置是否使用gps

        option.setLocationNotify(true);
        //可选，默认false，设置是否当GPS有效时按照1S/1次频率输出GPS结果

        option.setIsNeedLocationDescribe(true);
        //可选，默认false，设置是否需要位置语义化结果，可以在BDLocation.getLocationDescribe里得到，结果类似于“在北京天安门附近”

        option.setIsNeedLocationPoiList(true);
        //可选，默认false，设置是否需要POI结果，可以在BDLocation.getPoiList里得到

        option.setIgnoreKillProcess(false);
        //可选，默认true，定位SDK内部是一个SERVICE，并放到了独立进程，设置是否在stop的时候杀死这个进程，默认不杀死

        option.SetIgnoreCacheException(false);
        //可选，默认false，设置是否收集CRASH信息，默认收集

        option.setEnableSimulateGps(false);
        //可选，默认false，设置是否需要过滤GPS仿真结果，默认需要

        mLocationClient.setLocOption(option);
    }


    private void selfRequestPermission() {
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.M) {
            ArrayList<String> permissions = new ArrayList<String>();
            /***
             * 定位权限为必须权限，用户如果禁止，则每次进入都会申请
             */
            // 定位精确位置
            if (checkSelfPermission(Manifest.permission.ACCESS_FINE_LOCATION) != PackageManager.PERMISSION_GRANTED) {
                permissions.add(Manifest.permission.ACCESS_FINE_LOCATION);
            }
            if (checkSelfPermission(Manifest.permission.ACCESS_COARSE_LOCATION) != PackageManager.PERMISSION_GRANTED) {
                permissions.add(Manifest.permission.ACCESS_COARSE_LOCATION);
            }
            /*
             * 读写权限和电话状态权限非必要权限(建议授予)只会申请一次，用户同意或者禁止，只会弹一次
			 */
            // 读写权限
            if (addPermission(permissions, Manifest.permission.WRITE_EXTERNAL_STORAGE)) {
                permissionInfo += "Manifest.permission.WRITE_EXTERNAL_STORAGE Deny \n";
            }
            // 读取电话状态权限
            if (addPermission(permissions, Manifest.permission.READ_PHONE_STATE)) {
                permissionInfo += "Manifest.permission.READ_PHONE_STATE Deny \n";
            }

            if (permissions.size() > 0) {
                requestPermissions(permissions.toArray(new String[permissions.size()]), PERMISSION_STATUS);
            }

            if (permissions.size() == 0) {
                mLocationClient.start();
            }

        } else {
            mLocationClient.start();
        }
    }

    @TargetApi(23)
    private boolean addPermission(ArrayList<String> permissionsList, String permission) {
        if (checkSelfPermission(permission) != PackageManager.PERMISSION_GRANTED) { // 如果应用没有获得对应权限,则添加到列表中,准备批量申请
            if (shouldShowRequestPermissionRationale(permission)) {
                return true;
            } else {
                permissionsList.add(permission);
                return false;
            }

        } else {
            return true;
        }
    }


    @Override
    public void onRequestPermissionsResult(int requestCode, String[] permissions, int[] grantResults) {

        // TODO Auto-generated method stub
        super.onRequestPermissionsResult(requestCode, permissions, grantResults);
        mLocationClient.start();
    }


    @OnClick(R.id.mapdialog_close)
    public void SetMapInformation() {
        Intent returnIntent = new Intent();

        if(TextUtils.isEmpty(addr)){

        }
        else{
            returnIntent.putExtra("province",province);
            returnIntent.putExtra("city",city);
            returnIntent.putExtra("district",district);
            returnIntent.putExtra("street",street);
            returnIntent.putExtra("street_number",street_number);
        }

        setResult(Activity.RESULT_OK, returnIntent);
        finish();
    }



    @Override
    protected void onResume() {
        super.onResume();
        mBaiduMap.onResume();
    }

    @Override
    protected void onPause() {
        super.onPause();
        mBaiduMap.onPause();
    }

    @Override
    protected void onDestroy() {
        super.onDestroy();
        baiduMap.setMyLocationEnabled(false);
        mBaiduMap.onDestroy();
    }


    public class MyLocationListener implements BDLocationListener {

        @Override
        public void onReceiveLocation(final BDLocation location) {

            baiduMap.setMyLocationEnabled(true);
            // 构造定位数据
            MyLocationData locData = new MyLocationData.Builder()
                    .accuracy(location.getRadius())
                    // 此处设置开发者获取到的方向信息，顺时针0-360
                    .latitude(location.getLatitude())
                    .longitude(location.getLongitude()).build();
            // 设置定位数据
            baiduMap.setMyLocationData(locData);
            // 设置定位图层的配置（定位模式，是否允许方向信息，用户自定义定位图标）

            MyLocationConfiguration config = new MyLocationConfiguration(MyLocationConfiguration.LocationMode.NORMAL, true, mCurrentMarker);
            baiduMap.setMyLocationConfiguration(config);
            // 当不需要定位图层时关闭定位图层
            //baiduMap.setMyLocationEnabled(false);

            LatLng center = new LatLng(location.getLatitude(), location.getLongitude());

            MapStatus mMapStatus = new MapStatus.Builder()
                    .target(center)
                    .zoom(18)
                    .build();

            MapStatusUpdate mMapStatusUpdate = MapStatusUpdateFactory.newMapStatus(mMapStatus);
            baiduMap.animateMapStatus(mMapStatusUpdate);

            handler.post(new Runnable() {
                @Override
                public void run() {
                    progressBar.setVisibility(View.GONE);
                    addressInDetail.setVisibility(View.VISIBLE);
                    addressInDetail.setText(location.getAddrStr());
                }
            });

             province =location.getProvince();
             city=location.getCity();
             district = location.getDistrict();
             street = location.getStreet();
             street_number = location.getStreetNumber();
             addr = location.getAddrStr();



            //获取定位结果
            StringBuffer sb = new StringBuffer(256);

            sb.append("time : ");
            sb.append(location.getTime());    //获取定位时间

            sb.append("\nerror code : ");
            sb.append(location.getLocType());    //获取类型类型

            sb.append("\nlatitude : ");
            sb.append(location.getLatitude());    //获取纬度信息

            sb.append("\nlontitude : ");
            sb.append(location.getLongitude());    //获取经度信息

            sb.append("\nradius : ");
            sb.append(location.getRadius());    //获取定位精准度

            if (location.getLocType() == BDLocation.TypeGpsLocation) {

                // GPS定位结果
                sb.append("\nspeed : ");
                sb.append(location.getSpeed());    // 单位：公里每小时

                sb.append("\nsatellite : ");
                sb.append(location.getSatelliteNumber());    //获取卫星数

                sb.append("\nheight : ");
                sb.append(location.getAltitude());    //获取海拔高度信息，单位米

                sb.append("\ndirection : ");
                sb.append(location.getDirection());    //获取方向信息，单位度

                sb.append("\naddr : ");
                sb.append(location.getAddrStr());    //获取地址信息

                sb.append("\ndescribe : ");
                sb.append("gps定位成功");

            } else if (location.getLocType() == BDLocation.TypeNetWorkLocation) {

                // 网络定位结果
                sb.append("\naddr : ");
                sb.append(location.getAddrStr());    //获取地址信息

                sb.append("\noperationers : ");
                sb.append(location.getOperators());    //获取运营商信息

                sb.append("\ndescribe : ");
                sb.append("网络定位成功");

            } else if (location.getLocType() == BDLocation.TypeOffLineLocation) {

                // 离线定位结果
                sb.append("\ndescribe : ");
                sb.append("离线定位成功，离线定位结果也是有效的");

            } else if (location.getLocType() == BDLocation.TypeServerError) {

                sb.append("\ndescribe : ");
                sb.append("服务端网络定位失败，可以反馈IMEI号和大体定位时间到loc-bugs@baidu.com，会有人追查原因");

            } else if (location.getLocType() == BDLocation.TypeNetWorkException) {

                sb.append("\ndescribe : ");
                sb.append("网络不同导致定位失败，请检查网络是否通畅");

            } else if (location.getLocType() == BDLocation.TypeCriteriaException) {

                sb.append("\ndescribe : ");
                sb.append("无法获取有效定位依据导致定位失败，一般是由于手机的原因，处于飞行模式下一般会造成这种结果，可以试着重启手机");

            }

            sb.append("\nlocationdescribe : ");
            sb.append(location.getLocationDescribe());    //位置语义化信息

            List<Poi> list = location.getPoiList();    // POI数据
            if (list != null) {
                sb.append("\npoilist size = : ");
                sb.append(list.size());
                for (Poi p : list) {
                    sb.append("\npoi= : ");
                    sb.append(p.getId() + " " + p.getName() + " " + p.getRank());
                }
            }
            mLocationClient.unRegisterLocationListener(myListener);
            mLocationClient.stop();
            Log.d("Id", sb.toString());
        }

        @Override
        public void onConnectHotSpotMessage(String s, int i) {

        }

    }

    BaiduMap.OnMapClickListener listener = new BaiduMap.OnMapClickListener() {
        /**
         * 地图单击事件回调函数
         * @param point 点击的地理坐标
         */
        public void onMapClick(final LatLng point) {
            setMapCenter(point);
        }

        /**
         * 地图内 Poi 单击事件回调函数
         * @param poi 点击的 poi 信息
         */
        public boolean onMapPoiClick(MapPoi poi) {
            return true;
        }
    };


    private void setMapCenter(final LatLng center) {

        MapStatus mMapStatus = new MapStatus.Builder()
                .target(center)
                .zoom(18)
                .build();

        MapStatusUpdate mMapStatusUpdate = MapStatusUpdateFactory.newMapStatus(mMapStatus);
        baiduMap.animateMapStatus(mMapStatusUpdate);

    }

    private void GetLocationName(LatLng center) {
        WebClient client = new WebClient();

        client.setOnDataArrivedListener(new OnFinishedCallBack() {
            @Override
            public void onDataArrived(String result) {


                JSONTokener jsonParser = new JSONTokener(result);
                try {
                    JSONObject jsonResult = (JSONObject) jsonParser.nextValue();
                    String resultStatus = jsonResult.getString("status");
                    if (resultStatus.equals("0")) {
                        JSONObject resultObj = jsonResult.getJSONObject("result");
                        JSONObject addressComponent = resultObj.getJSONObject("addressComponent");
                        addr = resultObj.getString("formatted_address");


                        province =addressComponent.getString("province");
                        city=addressComponent.getString("city");
                        district = addressComponent.getString("district");
                        street = addressComponent.getString("street");
                        street_number = addressComponent.getString("street_number");

                        handler.post(new Runnable() {
                            @Override
                            public void run() {
                                progressBar.setVisibility(View.GONE);
                                addressInDetail.setVisibility(View.VISIBLE);
                                addressInDetail.setText(addr);
                            }
                        });

                    }

                } catch (JSONException e) {
                    e.printStackTrace();
                    handler.post(new Runnable() {
                        @Override
                        public void run() {
                            Toast.makeText(getApplicationContext(), "服务出错，请稍后再试", Toast.LENGTH_LONG).show();
                        }
                    });


                } finally {
                    handler.post(new Runnable() {
                        @Override
                        public void run() {
                            progressBar.setVisibility(View.GONE);
                            addressInDetail.setVisibility(View.VISIBLE);
                        }
                    });
                }

            }
        });

        try {
            String url = StaticParams.BAIDUAREA_URL.replace("###lat###", String.valueOf(center.latitude)).replace("###lng###", String.valueOf(center.longitude));
            client.GetData(url);
        } catch (IOException ex) {
            ex.printStackTrace();
            Log.e(Id, ex.getMessage());
            handler.post(new Runnable() {
                @Override
                public void run() {
                    Toast.makeText(MapActivity.this, "网络错误，请稍后重试", Toast.LENGTH_LONG).show();
                }
            });
        }
    }

}
