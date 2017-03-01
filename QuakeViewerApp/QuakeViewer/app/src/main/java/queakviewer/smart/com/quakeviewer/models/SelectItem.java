package queakviewer.smart.com.quakeviewer.models;

/**
 * Created by Ares on 2017/3/1.
 */

public class SelectItem extends Object {
    private String key;
    private String name;
    private String parentId;

    public String getParentId() {
        return parentId;
    }

    public void setParentId(String parentId) {
        this.parentId = parentId;
    }


    public String getKey() {
        return key;
    }

    public void setKey(String key) {
        this.key = key;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }


}
