rcsの作成

1. cygwinのX86版を取得する
 https://cygwin.com/install.html
 gccとmakeとedを動くようにすること
2. rcs5.9.4を作成する。
 この際 srcにpatched-srcを充てる
 ./configure
 lib/config.hのDIFFを変更
#define DIFF diff
 ./make
