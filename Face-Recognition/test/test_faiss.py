import numpy as np
import faiss
import time

if __name__ == '__main__':
    arr = np.random.randint(1, 10000, size=(200, 100)).astype(np.float32)
    index = faiss.IndexFlatL2(100)
    index = faiss.IndexIDMap(index)

    index.add_with_ids(arr[:100], np.array([102180171] * 100))
    index.add_with_ids(arr[100:], np.array([102180172] * 100))

    faiss.write_index(index, 'my.index')

    time.sleep(2)

    index = faiss.read_index('my.index')
    distances, ids = index.search(arr[0:1], 3)
    print(ids)