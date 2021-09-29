//Helper Function takes a character parameter and returns
//True: if the character is a digit between 0 and 9
//False: if the character is a non digit.
function IsDigit(c)
{
    return (c >= '0' && c <= '9');
}

//Helper Function takes a character parameter and returns
//True: if the character is a letter betweeen A-Z/a-z
//False: if the character is a non letter.
function IsLetter(c)
{
    return ((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z'));
}

//Helper Function takes two parameter, first the path represents the rout
//second is interval represents delay in milliseconds, 1000ms = 1s.
function Redirect(Path, Interval)
{
    window.setTimeout(function ()
    {
        window.location.href = Path;
    }, Interval);
}



//Add the Name Validation that is hooked with the server side validation.
jQuery.validator.addMethod('NameValidation', function (value, element)
{
    if (value == null || value.length == 0)
    {
        return false;
    }
    else
    {
        for (var i = 0; i < value.length; i++)
        {
            if (IsLetter(value[i]) == false)
            {
                return false;
            }
        }
    }
    return true;
});
$.validator.unobtrusive.adapters.addBool("NameValidation");


//Add the StudentId Validation that is hooked with the server side validation.
jQuery.validator.addMethod('StudentIdValidation', function (value, element)
{
    if (value == null || value.length != 9)
    {
        return false;
    }
    else
    {
        if ((value[0] == 'H') || (value[0] == 'h'))
        {
            for (var i = 1; i < value.length; i++)
            {
                if (IsDigit(value[i]) == false)
                {
                    return false;
                }
            }
        }
        else
        {
            return false;
        }
    }
    return true;
});
$.validator.unobtrusive.adapters.addBool("StudentIdValidation");

//Add the EmailValidation that is hooked with the server side validation.
jQuery.validator.addMethod('EmailValidation', function (value, element)
{
    const Domain = "hct.ac.ae";
    const IdLength = 9;

    if (value == null || value.includes("@") == false)
    {
        return false;
    }
    else
    {
        const Parts = value.split("@");
        if (Parts.length != 2)
        {
            return false;
        }
        else
        {
            let CurrentId = Parts[0];
            let CurrentDomain = Parts[1];

            if (CurrentDomain != Domain || CurrentId.length != IdLength)
            {
                return false;
            }
            else
            {
                if (CurrentId[0] != 'H' && CurrentId[0] != 'h')
                {
                    return false;
                }
                else
                {
                    for (var i = 1; i < CurrentId.length; i++)
                    {
                        if (IsDigit(CurrentId[i]) == false)
                        {
                            return false;
                        }
                    }
                }
            }
        }
    }
    return true;
});
$.validator.unobtrusive.adapters.addBool("EmailValidation");


//Handle the "Back to main page" Button
$(document).ready(function ()
{
    $('#BackToMainButton').on('click', e =>
    {
        Redirect('/', 1000);
    });
});

//Handle the Sign Up Register Button POST's request using AJAX
$(document).ready(function ()
{
    $("#SignUpButton").on('click', e =>
    {
        e.preventDefault();
        $("#SignUpForm").validate();
        if ($("#SignUpForm").valid())
        {
            $.ajax
            ({
                type: "POST",
                url: 'SignUp?',
                data: $("#SignUpForm").serialize(),
                dataType: "json",
                success: function (response)
                {
                    if (response != null)
                    {
                        console.log(response);

                        if (response.state === 'Valid')
                        {
                            $('#ConfirmationModal').modal('show');
                            document.getElementById('SignUpResult').textContent = '';
                        }
                        else if (response.state === 'Error')
                        {
                            document.getElementById('SignUpResult').textContent = 'The Account is already registered.';
                        }
                        else
                        {
                            console.log('Received a failure state');
                        }
                    }
                    else
                    {
                        alert("Something went wrong");
                    }
                }
            });
        }
        else
        {
            console.log('Validation failed');
        } 
    });
});

//Handle the Sign Up Confirmation Button POST's request using AJAX
$(document).ready(function () {
    $("#SubmitCodeButton").on('click', e =>
    {
        e.preventDefault();
        console.log("Submit Code Button Clicked");
        $.ajax
        ({
            type: "POST",
            url: 'SignUp/Check',
            data: $("#SignUpForm").serialize(),
            dataType: "json",
            success: function (response)
            {
                if (response != null)
                {
                    console.log(response);
                    let ConfirmationResult = document.getElementById('ConfirmationResult');
                    if (response.state === 'Failed')
                    {
                        ConfirmationResult.classList.replace("text-success", "text-danger");
                        ConfirmationResult.textContent = 'Invalid Confirmation Code';
                    }
                    else if (response.state === 'Success')
                    {
                        ConfirmationResult.classList.replace("text-danger", "text-success");
                        ConfirmationResult.textContent = 'Redirection..';
                        Redirect('/SignUp/Successful', 2500);
                    }
                }
                else
                {
                    alert("Something went wrong");
                }
            }
        });
    });
});