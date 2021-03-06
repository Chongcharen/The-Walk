//
//  TWTRConstants.h
//
//  Copyright (c) 2014 Twitter. All rights reserved.
//

#import <Foundation/Foundation.h>

#pragma mark - Error messages

/**
 * The NSError domain of errors surfaced by the Twitter SDK.
 */
FOUNDATION_EXPORT NSString * const TWTRErrorDomain;

/**
 *  Error codes surfaced by the Twitter SDK.
 */
typedef NS_ENUM(NSInteger, TWTRErrorCode) {
    /**
     *  Unknown error.
     */
    TWTRErrorCodeUnknown = -1,
    /**
     *  Authentication has not been set up yet. You must call -[Twitter logInWithCompletion:] or -[Twitter logInGuestWithCompletion:]
     */
    TWTRErrorCodeNoAuthentication = 0,
    /**
     *  Twitter has not been initialized yet. Call +[Fabric with:@[TwitterKit]] or -[Twitter startWithConsumerKey:consumerSecret:].
     */
    TWTRErrorCodeNotInitialized = 1,
    /**
     *  User has declined to grant permission to information such as their email address.
     */
    TWTRErrorCodeUserDeclinedPermission = 2,
    /**
     *  User has granted permission to their email address but no address is associated with their account.
     */
    TWTRErrorCodeUserHasNoEmailAddress = 3,
    /**
     *  A resource has been requested by ID, but that ID was not found.
     */
    TWTRErrorCodeInvalidResourceID = 4
};

/**
 *  The NSError domain of errors surfaced by the Twitter SDK during the login operation.
 */
FOUNDATION_EXPORT NSString * const TWTRLogInErrorDomain;

/**
 *  Error codes surfaced by the Twitter SDK with the `TWTRLogInErrorDomain` error domain.
 */
typedef NS_ENUM(NSInteger, TWTRLogInErrorCode) {
    /**
     * Unknown error.
     */
    TWTRLogInErrorCodeUnknown = -1,
    /**
     * User denied login.
     */
    TWTRLogInErrorCodeDenied = 0,
    /**
     * User canceled login.
     */
    TWTRLogInErrorCodeCanceled = 1,
    /**
     * No Twitter account found.
     */
    TWTRLogInErrorCodeNoAccounts = 2,
    /**
     * Reverse auth with linked account failed.
     */
    TWTRLogInErrorCodeReverseAuthFailed = 3
};
