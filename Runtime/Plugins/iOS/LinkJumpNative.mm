#import <Foundation/Foundation.h>

extern "C" bool LinkJump_CanOpenUrl(const char* url)
{
    if (url == NULL)
        return false;
    
    NSString* strUrl = [NSString stringWithUTF8String: url];
    
    return [[UIApplication sharedApplication] canOpenURL:[NSURL URLWithString:strUrl]];
}
