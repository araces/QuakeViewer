package queakviewer.smart.com.quakeviewer.Utils;

import android.app.Dialog;
import android.content.Context;
import android.graphics.Color;
import android.graphics.drawable.ColorDrawable;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.Button;
import android.widget.ListAdapter;
import android.widget.ListView;
import android.widget.TextView;

import java.util.ArrayList;
import java.util.List;

import butterknife.BindView;
import butterknife.ButterKnife;
import queakviewer.smart.com.quakeviewer.R;
import queakviewer.smart.com.quakeviewer.models.MsgDialogModel;

/**
 * Created by Ares on 2017/3/14.
 */


public class MsgDialog extends Dialog {

    @BindView(R.id.msg_view)
    ListView msgList;

    @BindView(R.id.dialog_close)
    Button dialog_close;

    private List<MsgDialogModel> irons =new ArrayList<MsgDialogModel>();
    private List<MsgDialogModel> shuini =new ArrayList<MsgDialogModel>();
    private List<ArrayList<MsgDialogModel>> brikes =new ArrayList<ArrayList<MsgDialogModel>>();
    private List<MsgDialogModel> stones =new ArrayList<MsgDialogModel>();



    public MsgDialog(Context context,int type){
        super(context);
        initData();
        dialog_close.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                MsgDialog.this.dismiss();
            }
        });
        if(type == 0){
            LisMsgAdapter adapter =new LisMsgAdapter(context,irons,null,0);
            msgList.setAdapter(adapter);
        }else if(type == 1){
            LisMsgAdapter adapter =new LisMsgAdapter(context,shuini,null,0);
            msgList.setAdapter(adapter);
        }else if(type == 2){
            LisMsgAdapter adapter =new LisMsgAdapter(context,null,brikes,1);
            msgList.setAdapter(adapter);
        }else{
            LisMsgAdapter adapter =new LisMsgAdapter(context,stones,null,0);
            msgList.setAdapter(adapter);
        }

    }

    class LisMsgAdapter extends BaseAdapter{

        Context mContext;
        List<MsgDialogModel> mSource;
        List<ArrayList<MsgDialogModel>> mSource1;
        private LayoutInflater mInflater;
        int mode=0;

        public  LisMsgAdapter(Context context,List<MsgDialogModel> source,List<ArrayList<MsgDialogModel>> source1,int mode){
            this.mContext = context;
            this.mSource = source;
            this.mSource1 = source1;
            mInflater =  LayoutInflater.from(context);
            this.mode = mode;
        }
        @Override
        public int getCount() {
            if(this.mode==0) {
                return mSource.size();
            }else{
                return mSource1.size();
            }
        }

        @Override
        public Object getItem(int i) {
            if(this.mode==0) {
                return mSource.get(i);
            }
            else{
                return mSource1.get(i);
            }
        }

        @Override
        public long getItemId(int i) {
            return i;
        }

        @Override
        public View getView(int i, View view, ViewGroup viewGroup) {

            if(this.mode==0) {
                ViewHolder holder = null;
                if (view == null) {

                    holder = new ViewHolder();

                    view = mInflater.inflate(R.layout.resource_item, null);
                    holder.img = (XCRoundRectImageView) view.findViewById(R.id.resource_item_img);
                    holder.title = (TextView) view.findViewById(R.id.resource_item_title);

                    view.setTag(holder);

                } else {

                    holder = (ViewHolder) view.getTag();
                }


                holder.img.setImageResource(mSource.get(i).getImageId());
                holder.title.setText(mSource.get(i).getImageName());
            }
            else{
                ViewHolderDouble doubleHolder = null;
                if (view == null) {

                    doubleHolder = new ViewHolderDouble();

                    view = mInflater.inflate(R.layout.resource_double_item, null);
                    doubleHolder.img_left = (XCRoundRectImageView) view.findViewById(R.id.resource_item_img_left);
                    doubleHolder.title_left = (TextView) view.findViewById(R.id.resource_item_title_left);
                    doubleHolder.img_Right = (XCRoundRectImageView) view.findViewById(R.id.resource_item_img_right);
                    doubleHolder.title_Right = (TextView) view.findViewById(R.id.resource_item_title_right);
                    view.setTag(doubleHolder);

                } else {

                    doubleHolder = (ViewHolderDouble) view.getTag();
                }


                doubleHolder.img_left.setImageResource(mSource1.get(i).get(0).getImageId());
                doubleHolder.title_left.setText(mSource1.get(i).get(0).getImageName());
                doubleHolder.img_Right.setImageResource(mSource1.get(i).get(1).getImageId());
                doubleHolder.title_Right.setText(mSource1.get(i).get(1).getImageName());
            }
            return view;

        }
    }

    public final class ViewHolder{
        public XCRoundRectImageView img;
        public TextView title;
    }

    public final class ViewHolderDouble{
        public XCRoundRectImageView img_left;
        public TextView title_left;
        public XCRoundRectImageView img_Right;
        public TextView title_Right;
    }


    private void initData(){
        /**设置对话框背景透明*/
        getWindow().setBackgroundDrawable(new ColorDrawable(Color.TRANSPARENT));
        setContentView(R.layout.view_dialog);
        ButterKnife.bind(this);
        setCanceledOnTouchOutside(true);

        MsgDialogModel iron1 =new MsgDialogModel();
        iron1.setIndex(0);
        iron1.setImageId(R.drawable.iron1);
        iron1.setImageName("钢结构1");
        irons.add(0,iron1);

        MsgDialogModel iron2 =new MsgDialogModel();
        iron2.setIndex(1);
        iron2.setImageId(R.drawable.iron2);
        iron2.setImageName("钢结构2");
        irons.add(1,iron2);

        MsgDialogModel iron3 =new MsgDialogModel();
        iron3.setIndex(2);
        iron3.setImageId(R.drawable.iron3);
        iron3.setImageName("钢结构3");
        irons.add(2,iron3);


        MsgDialogModel shuini1 =new MsgDialogModel();
        shuini1.setIndex(0);
        shuini1.setImageId(R.drawable.concrete1);
        shuini1.setImageName("钢筋混凝土1");
        shuini.add(0,shuini1);

        MsgDialogModel shuini2 =new MsgDialogModel();
        shuini2.setIndex(1);
        shuini2.setImageId(R.drawable.concrete2);
        shuini2.setImageName("钢筋混凝土2");
        shuini.add(1,shuini2);

        MsgDialogModel shuini3 =new MsgDialogModel();
        shuini3.setIndex(2);
        shuini3.setImageId(R.drawable.concrete3);
        shuini3.setImageName("钢筋混凝土3");
        shuini.add(2,shuini3);


        ArrayList<MsgDialogModel> brikeLine1=new ArrayList<MsgDialogModel>();
        MsgDialogModel brikeLeft1 =new MsgDialogModel();
        brikeLeft1.setIndex(0);
        brikeLeft1.setImageId(R.drawable.brike1);
        brikeLeft1.setImageName("砖砌1");
        brikeLine1.add(0,brikeLeft1);

        MsgDialogModel brikeRight1 =new MsgDialogModel();
        brikeRight1.setIndex(1);
        brikeRight1.setImageId(R.drawable.brike2);
        brikeRight1.setImageName("砖砌2");
        brikeLine1.add(1,brikeRight1);

        brikes.add(0,brikeLine1);

        ArrayList<MsgDialogModel> brikeLine2=new ArrayList<MsgDialogModel>();
        MsgDialogModel brikeLeft2 =new MsgDialogModel();
        brikeLeft2.setIndex(2);
        brikeLeft2.setImageId(R.drawable.brike3);
        brikeLeft2.setImageName("砖砌3");
        brikeLine2.add(0,brikeLeft2);

        MsgDialogModel brikeRight2 =new MsgDialogModel();
        brikeRight2.setIndex(3);
        brikeRight2.setImageId(R.drawable.brike4);
        brikeRight2.setImageName("砖砌4");
        brikeLine2.add(1,brikeRight2);

        brikes.add(1,brikeLine2);

        ArrayList<MsgDialogModel> brikeLine3=new ArrayList<MsgDialogModel>();

        MsgDialogModel brikeLeft3 =new MsgDialogModel();
        brikeLeft3.setIndex(4);
        brikeLeft3.setImageId(R.drawable.brike5);
        brikeLeft3.setImageName("砖砌5");
        brikeLine3.add(0,brikeLeft3);

        MsgDialogModel brikeRight3 =new MsgDialogModel();
        brikeRight3.setIndex(5);
        brikeRight3.setImageId(R.drawable.brike6);
        brikeRight3.setImageName("砖砌6");
        brikeLine3.add(1,brikeRight3);

        brikes.add(2,brikeLine3);

        MsgDialogModel stone1 =new MsgDialogModel();
        stone1.setIndex(0);
        stone1.setImageId(R.drawable.stone1);
        stone1.setImageName("土石1");
        stones.add(0,stone1);

        MsgDialogModel stone2 =new MsgDialogModel();
        stone2.setIndex(1);
        stone2.setImageId(R.drawable.concrete2);
        stone2.setImageName("土石2");
        stones.add(1,stone2);

        MsgDialogModel stone3 =new MsgDialogModel();
        stone3.setIndex(2);
        stone3.setImageId(R.drawable.concrete3);
        stone3.setImageName("土石3");
        stones.add(2,stone3);
    }

}
