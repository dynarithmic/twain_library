#include "JavaAdapter.h"

JavaAdapter::JavaAdapter(JNIEnv* jEnv, jobject jObj) 
{
    Init(jEnv, jObj);
};


JavaAdapter::~JavaAdapter() 
{
    if ( jObject )
        getJNIEnv()->DeleteGlobalRef(jObject);
}

JNIEnv* JavaAdapter::getJNIEnv() 
{
    JNIEnv* jEnv = NULL;
    
    int retVal = pJavaVM->AttachCurrentThread((void**) &jEnv, NULL);
    if(retVal) {
//        printf("AttachCurrentThread error %d", retVal);
    }
    return jEnv;
};

void JavaAdapter::releaseJNIEnv() 
{
    int retVal = pJavaVM->DetachCurrentThread();
    if(retVal) {
//        printf("DetachCurrentThread error %d", retVal);
    }
};

void JavaAdapter::Init(JNIEnv* jEnv, jobject jObj)
{
    pJavaVM = NULL;

    int retVal = jEnv->GetJavaVM(&pJavaVM);
    if (retVal) {
  //      printf("GetJavaVM error %d", retVal);
    }
    jObject = jEnv->NewGlobalRef(jObj);
}    

void JavaAdapter::Destroy()
{
    JNIEnv *jEnv = getJNIEnv();
    if ( jEnv )
    {
        if ( jObject )
            jEnv->DeleteGlobalRef(jObject);
        releaseJNIEnv();
    }
}

