package queakviewer.smart.com.quakeviewer.Utils;

import java.io.IOException;

import okhttp3.MediaType;
import okhttp3.OkHttpClient;
import okhttp3.Request;
import okhttp3.RequestBody;
import okhttp3.Response;

/**
 * Created by Ares on 2017/2/25.
 */

public class WebClient  {

    private OnFinishedCallBack mOnFinishedCallBackListener;

    public void setOnDataArrivedListener(OnFinishedCallBack mOnFinishedCallBack){
        this.mOnFinishedCallBackListener = mOnFinishedCallBack;
    }

    OkHttpClient client = new OkHttpClient();
    public static final MediaType JSON
            = MediaType.parse("application/json; charset=utf-8");



    public void PostData(String url, String jsonParams) throws IOException{
        RequestBody body =RequestBody.create(JSON,jsonParams);
        Request request = new Request.Builder()
                .url(url)
                .post(body)
                .build();
        Response response = client.newCall(request).execute();

        if(response.isSuccessful()){
            if(null != mOnFinishedCallBackListener){
                mOnFinishedCallBackListener.onDataArrived(response.body().string());
            }
        }else{
            throw new IOException("Unexpected code " + response);
        }
    }

   public void GetData(String url) throws IOException {

        Request request = new Request.Builder().url(url).build();
        Response response = client.newCall(request).execute();
        if(response.isSuccessful()){
            if(null != mOnFinishedCallBackListener){
                mOnFinishedCallBackListener.onDataArrived(response.body().string());
            }
        }else{
            throw new IOException("Unexpected code " + response);
        }
    }
}
