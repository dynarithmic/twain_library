#include <jni.h>
class JavaAdapter {
    private:
        JavaAdapter() {};
        
    public:
        JavaVM*   pJavaVM;
        jobject   jObject;

        JNIEnv * getJNIEnv();
        void releaseJNIEnv();

    public:
        JavaAdapter(JNIEnv * jEnv, jobject jObj = 0);
        void Init(JNIEnv * jEnv, jobject jObj = 0);
        void Destroy();
        virtual ~JavaAdapter();
};
