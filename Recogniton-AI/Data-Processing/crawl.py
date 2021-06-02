import numpy as np
import requests as req
import os

faculty_codes = {
    'Co-Khi': '101',
    'Cong-nghe-thong-tin': '102',
    'Co-khi-giao-thong': '103',
    'Cong-nghe-nhiet-lanh': '104',
    'Dien': '105',
    'Dien-tu-vien-thong': '106',
    'Hoa': '107',
    'Xay-dung-cau-duong': '109',
    'Xay-dung-dan-dung-cong-nghiep': '110',
    'Xay-dung-cong-trinh-thuy': '111',
    'Moi-truong': '117',
    'Quan-ly-du-an': '118',
    'Kien-truc': '121',
    'Cong-nghe-tien-tien-PFIEV': '122',
    'Cong-nghe-tien-tien': '123',
}

years = (np.arange(11, 21) * 10).astype(str)

max_students = 300
student_serials = [f'{i:03}' for i in range(1, max_students + 1)]

def downloadImage(url, folder):
    res = req.get(url)
    if res.ok is True:
        data = res.content
        file = folder + '/' + url[url.rfind('/') + 1::]
        with open(file, 'wb') as handler:
            print('Downloading image ' + file)
            handler.write(data)

def createUrl(year, id_student):
    return 'http://cb.dut.udn.vn/imagesv/' + str(int(year) // 10) + '/' + id_student + '.jpg'

for faculty in faculty_codes:
    if not os.path.exists(faculty):
        os.mkdir(faculty)
    
    for year in years:
        folder = faculty + '/K' + str(int(year) // 10)
        if not os.path.exists(folder):
            os.mkdir(folder)
        
        for serial in student_serials:
            id_student = faculty_codes[faculty] + year + serial
            url = createUrl(year, id_student)
            downloadImage(url, folder)
            
print('========================DONE=======================')
