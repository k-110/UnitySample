#import


memo_file = 'memo/memo.txt'

# read_memo
def read_memo():
    f = open(memo_file, 'r', encoding='UTF-8')
    data = f.read()
    f.close()
    return data

# write_memo
def write_memo(data):
    f = open(memo_file, 'w', encoding='UTF-8')
    f.write(data)
    f.close()
