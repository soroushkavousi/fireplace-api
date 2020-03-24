using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamingCommunityApi.Core.Enums
{
    public enum ErrorName
    {
        // Main business logic errors
        BAD_REQUEST,
        AUTHENTICATION_FAILED,
        OLD_PASSWORD_NOT_CORRECT,
        EMAIL_ACTIVATION_CODE_NOT_CORRECT,


        // Access Denied errors
        ACCESS_DENIED,
        EMAIL_ACCESS_DENIED,


        // Existing errors
        USER_ID_EXISTS,
        USER_ID_DOES_NOT_EXIST,
        USERNAME_EXISTS,
        USERNAME_DOES_NOT_EXIST,
        EMAIL_ADDRESS_EXISTS,
        EMAIL_ADDRESS_DOES_NOT_EXIST,
        EMAIL_ID_EXISTS,
        EMAIL_ID_DOES_NOT_EXIST,


        // Format errors
        REQUEST_BODY_IS_NOT_JSON,
        PASSWORD_MIN_LENGTH,
        PASSWORD_MAX_LENGTH,
        PASSWORD_AN_UPPERCASE_LETTER,
        PASSWORD_A_NUMBER,
        PASSWORD_A_LOWERCASE_LETTER,
        PASSWORD_VALID_CHARACTERS,

        USERNAME_MIN_LENGTH,
        USERNAME_MAX_LENGTH,
        USERNAME_WRONG_START,
        USERNAME_WRONG_END,
        USERNAME_INVALID_CONSECUTIVE,
        USERNAME_VALID_CHARACTERS,

        EMAIL_ADDRESS_NOT_VALID,

        FIRST_NAME_NOT_VALID,
        LAST_NAME_NOT_VALID,


        // Missing parameter(s) errors
        FIRST_NAME_IS_NULL,
        LAST_NAME_IS_NULL,
        USERNAME_IS_NULL,
        PASSWORD_IS_NULL,
        EMAIL_ADDRESS_IS_NULL,
        USER_ID_IS_NULL,
        REQUIRED_BOTH_OF_OLD_PASSWORD_AND_PASSWORD,
        EMAIL_ID_IS_NULL,
        ACTIVATION_CODE_IS_NULL,

        INTERNAL_SERVER,
    }
}
