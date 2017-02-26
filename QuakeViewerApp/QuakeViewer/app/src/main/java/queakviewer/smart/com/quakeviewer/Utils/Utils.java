package queakviewer.smart.com.quakeviewer.Utils;

import android.content.Context;
import android.content.SharedPreferences;

/**
 * Created by Ares on 2017/2/25.
 */

public class Utils {
    private static final String MainKey = "USER_INFORMATION";

    public static void SaveContent(Context context, String key, String content){
        SharedPreferences sharedPreferences = context.getSharedPreferences(MainKey,Context.MODE_PRIVATE);
        SharedPreferences.Editor editor = sharedPreferences.edit();
        editor.putString(key,content);
        editor.commit();
    }

    public static  String GetContent(Context context,String key){
        SharedPreferences sharedPreferences = context.getSharedPreferences(MainKey,Context.MODE_PRIVATE);
        return sharedPreferences.getString(key,"");
    }
}
