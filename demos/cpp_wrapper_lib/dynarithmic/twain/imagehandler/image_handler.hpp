/*
This file is part of the Dynarithmic TWAIN Library (DTWAIN).
Copyright (c) 2002-2022 Dynarithmic Software.

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
*/
#ifndef DTWAIN_IMAGE_HANDLER_HPP
#define DTWAIN_IMAGE_HANDLER_HPP

#include <ostream>
#include <vector>
#include <memory>
#include <utility>
#ifndef DTWAIN_NOIMPORTLIB 
#include <dtwain.h>
#else
    #include <dtwainx2.h>
#endif

#include <dynarithmic/twain/dtwain_twain.hpp>

namespace dynarithmic
{
    namespace twain
    {
        struct image_information
        {
            double x_resolution = 0;
            double y_resolution = 0;
            long width = 0;
            long length = 0;
            long numsamples = 0;
            std::vector<int> bitsPerSample;
            long bitsPerPixel = 0;
            bool planar = false;
            long pixelType = 0;
            long compression = 0;
            friend std::ostream& operator <<(std::ostream& os, const image_information& ii);
        };

        // image handler class that is created after a device acquires images to memory.
        // Note that this has only been tested in Windows, as it uses the Device Independent
        // Bitmap (DIB) type.  
        class image_handler
        {
            using images_vector = std::vector<std::vector<HANDLE>>;
            std::shared_ptr<images_vector> vect_image_handle_ptr;
            std::vector<HANDLE> dummy;
            bool m_bAutoDestroy;

        public:
            image_handler(bool containsImages=true) : vect_image_handle_ptr(containsImages ? new images_vector : nullptr),
                                                                       m_bAutoDestroy(false)
            {}

            image_handler& set_contains_images(bool bSet)
            {
                if ( bSet && !vect_image_handle_ptr )
                    vect_image_handle_ptr = std::make_shared<images_vector>();
                else
                if (!bSet)
                {
                    if ( vect_image_handle_ptr )
                        vect_image_handle_ptr->clear();
                }
                return *this;
            }

            size_t get_num_acquisitions() const { return vect_image_handle_ptr?vect_image_handle_ptr->size():0; }
            size_t size() const { return get_num_acquisitions(); }

            const std::vector<HANDLE>& operator[](size_t acq_number) const
            {
                return get_acquisition_images(acq_number);
            }

            image_handler& set_auto_destroy(bool b)
            {
                m_bAutoDestroy = b;
                return *this;
            }

            size_t get_num_pages(size_t acq_number) const
            {
                if (!vect_image_handle_ptr)
                    return 0;
                if (acq_number >= vect_image_handle_ptr->size())
                    return 0;
                return (*vect_image_handle_ptr)[acq_number].size();
            }

            const std::vector<HANDLE>& get_acquisition_images(size_t acq_number) const
            {
                if (get_num_pages(acq_number) == 0)
                    return dummy;
                return (*vect_image_handle_ptr)[acq_number];
            }

            // Return a specific DIB
            HANDLE get_image_handle(size_t acquisition, size_t page) const
            {
                auto& images = get_acquisition_images(acquisition);
                if (page < images.size())
                    return images[page];
                return nullptr;
            }

            HANDLE operator() (size_t row, size_t col) const
            {
                return get_image_handle(row, col);
            }

            static int CalculateUsedPaletteEntries(int bit_count) {
                if ((bit_count >= 1) && (bit_count <= 8))
                    return 1 << bit_count;
                return 0;
            }

            std::vector<unsigned char> get_image_as_BMP(HANDLE hDib) const
            {
                std::vector<unsigned char> retval;
                if (!hDib)
                    return retval;
                BITMAPFILEHEADER fileheader;
                LPBITMAPINFOHEADER lpbi = NULL;
                memset((char *)&fileheader, 0, sizeof(BITMAPFILEHEADER));

                fileheader.bfType = 0x4D42;

                /* Fill in the fields of the file header */
                BYTE *pImage2 = (BYTE *)GlobalLock(hDib);

                lpbi = reinterpret_cast<LPBITMAPINFOHEADER>(pImage2);

                fileheader.bfSize = (DWORD)GlobalSize(hDib) + sizeof(BITMAPFILEHEADER);
                fileheader.bfReserved1 = 0;
                fileheader.bfReserved2 = 0;
                fileheader.bfOffBits = (DWORD)sizeof(BITMAPFILEHEADER) +
                    lpbi->biSize + CalculateUsedPaletteEntries(lpbi->biBitCount) * sizeof(RGBQUAD);

                // Write the file header
                char *ptrFileheader = reinterpret_cast<char *>(&fileheader);
                std::copy(ptrFileheader, ptrFileheader + sizeof(BITMAPFILEHEADER), std::back_inserter(retval));

                // Write the data
                DWORD gSize = static_cast<DWORD>(GlobalSize(hDib));
                std::copy(pImage2, pImage2 + gSize, std::back_inserter(retval));
                return retval;
            }

            HANDLE flip_BMP_image(HANDLE hDib)
            {
                API_INSTANCE DTWAIN_FlipBitmap(hDib);
                return hDib;
            }

            // Return a DIB as a BMP in memory
            std::vector<unsigned char> get_image_as_BMP(size_t acquisition, size_t page) const
            {
                return get_image_as_BMP(get_image_handle(acquisition, page));
            }

            void add_new_acquisition()
            {
                if (vect_image_handle_ptr)
                    vect_image_handle_ptr->resize(vect_image_handle_ptr->size() + 1);
            }

            void push_back_image(HANDLE h)
            {
                if (vect_image_handle_ptr)
                    vect_image_handle_ptr->back().push_back(h);
            }

            void destroy_image_handles()
            {
                if (vect_image_handle_ptr)
                {
                    auto iter = vect_image_handle_ptr->begin();
                    while (iter != vect_image_handle_ptr->end())
                    {
                        auto& vImages = *iter;
                        auto inner = vImages.begin();
                        while (inner != vImages.end())
                        {
                            ::GlobalUnlock(*inner);
                            ::GlobalFree(*inner);
                            ++inner;
                        }
                        ++iter;
                    }
                    vect_image_handle_ptr->clear();
                }
            }

            ~image_handler()
            {
                if (m_bAutoDestroy && vect_image_handle_ptr && vect_image_handle_ptr.use_count() == 1)
                    destroy_image_handles();
            }
        };
    }
}
#endif
