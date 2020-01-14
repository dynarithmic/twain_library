/*
This file is part of the Dynarithmic TWAIN Library (DTWAIN).
Copyright (c) 2002-2020 Dynarithmic Software.

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.

FOR ANY PART OF THE COVERED WORK IN WHICH THE COPYRIGHT IS OWNED BY
DYNARITHMIC SOFTWARE. DYNARITHMIC SOFTWARE DISCLAIMS THE WARRANTY OF NON INFRINGEMENT
OF THIRD PARTY RIGHTS.

For more information, the license file named LICENSE that is located in the root
directory of the DTWAIN installation covers the restrictions under the LGPL license.
Please read this file before deploying or distributing any application using DTWAIN.
*/
#ifndef PDFENCRYPT_H
#define PDFENCRYPT_H
#include <vector>
#include <string>
#include <md5class.h>
#include <MD5Checksum.h>
//#include "..\cryptolib\md5.h"
//#include "..\cryptolib\aes.h"

#define ENCRYPTION_OK           0
#define ENCRYPTION_NOTSET       1
#define ENCRYPTION_ERROR        2
#define ENCRYPTION_UNAVAILABLE  3

class PDFEncryption 
{
    public:
        typedef std:: vector<unsigned char> UCHARArray;

    protected:
    static unsigned char pad[32];
        UCHARArray state;
        int m_xRC4Component;
        int m_yRC4Component;
    
        /** The encryption key for a particular object/generation */
        UCHARArray key;
        /** The encryption key length for a particular object/generation */
        int keySize;

        /** The global encryption key */
        UCHARArray mkey;

        /** Alternate message digest */
        //CMD5Checksum MD5Alternate;

        /** The encryption key for the owner */
        UCHARArray ownerKey;
    
        /** The encryption key for the user */
        UCHARArray userKey;

        int permissions;
        
        std:: string m_documentID;

        UCHARArray PadPassword(const UCHARArray& passw);
        void SetupByUserPad(const std::string& documentID, 
                            const UCHARArray& userPad, 
                            const UCHARArray& ownerKey, 
                            int permissions, 
                            bool strength128Bits);
        void SetupGlobalEncryptionKey(const std::string& documentID, 
                                      const UCHARArray& userPad, 
                                      const UCHARArray& ownerKey, 
                                      int permissions, 
                                       bool strength128Bits) ;
        void SetupUserKey();
		void PrepareRC4Key(const UCHARArray& key);
		void PrepareRC4Key(const UCHARArray& key, int off, int len);
		void EncryptRC4(const UCHARArray& dataIn, int off, int len, UCHARArray& dataOut);
		void EncryptRC4(UCHARArray& data, int off, int len);
		void EncryptRC4(const UCHARArray& dataIn, UCHARArray& dataOut);
		void EncryptRC4(UCHARArray& data);
		virtual UCHARArray GetExtendedKey(int number, int generation);

    public:
        PDFEncryption(); 
		virtual ~PDFEncryption() {}
        UCHARArray ComputeOwnerKey(const UCHARArray& userPad, 
                                   const UCHARArray& ownerPad, 
                                   bool strength128Bits);

        void SetupAllKeys(const std:: string& DocID,
                          const std:: string& userPassword, 
                          const std:: string& ownerPassword, int permissions, 
                          bool strength128Bits);

        void SetupAllKeys(const std:: string& DocID,
                          UCHARArray& userPassword, UCHARArray& ownerPassword, 
                          int permissions, bool strength128Bits); 
        virtual void PrepareKey() = 0;
        void SetHashKey(int number, int generation);
        std:: string CreateEncryptionDictionary();
		virtual void Encrypt(const std::string& /*dataIn*/, std::string& /*dataOut*/) {}
		virtual void Encrypt(char * /*dataIn*/, int/* len*/) {}

        UCHARArray& GetUserKey() { return userKey; }
        UCHARArray& GetOwnerKey() { return ownerKey; }
        UCHARArray& GetEncryptionKey() { return mkey; }
        int GetPermissions() const { return permissions; }
};

class PDFEncryptionRC4 : public PDFEncryption
{
	protected:
		UCHARArray GetExtendedKey(int number, int generation);
	public:
		void Encrypt(const std::string& dataIn, std::string& dataOut);
		void Encrypt(char *dataIn, int len);
		void PrepareKey();

};
/*
class PDFEncryptionAES: public PDFEncryption
{
	protected:
		UCHARArray GetExtendedKey(int number, int generation);
	private:
		unsigned char m_ivValue[CryptoPP::AES::BLOCKSIZE];
	public:
		void Encrypt(const std::string& dataIn, std::string& dataOut);
//		void Encrypt(char *dataIn, int len);
		void PrepareKey();
};*/

#endif