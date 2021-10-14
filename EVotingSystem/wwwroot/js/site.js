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

//Helper Function takes one parameter that represents the Id of the modal.
//The function hides the modal.
function HideModal(Id)
{
    $(Id).modal('hide');
}

//Helper Function that resets all of the inputs from the sign up
function ResetSignUp()
{
    let SignUpForm = document.getElementById('SignUpForm');
    if (SignUpForm != null)
    {
        document.getElementById('FirstNameTextBox').value = '';
        document.getElementById('LastNameTextBox').value = '';
        document.getElementById('StudentIdTextBox').value = '';
        document.getElementById('NationalIdTextBox').value = '';
        document.getElementById('EmailTextBox').value = '';
        document.getElementById('PasswordTextBox').value = '';
        document.getElementById('PhoneTextBox').value = '';
    }
}

//Helper Function takes two parameters, that represents the Id of the Control, Content of the Control.
//The function adds a spinner code and changes the text.
function ShowSpinner(Id, Content)
{
    console.log('Starting ShowSpinner Function');
    let Button = document.getElementById(Id);
    //Disable Button
    Button.setAttribute("disabled", "disabled");
    //Inject Spinner Code and Replace the Text
    Button.innerHTML = Button.innerHTML.replace(Content, "Loading..");
    Button.innerHTML = Button.innerHTML.concat('<div class="spinner-border" role="status" id="spinner"><span class="sr-only">Loading...</span></div>');
    console.log('Finished ShowSpinner Function');
}

//Helper Function takes two parameters, that represents the Id of the Control, Content of the Control.
//The function removes the spinner code and changes the text.
function HideSpinner(Id, Content)
{
    let Button = document.getElementById(Id);
    //Enable Button
    Button.removeAttribute("disabled");
    //Remove all of the Injected Spinner Code and Add a new Text.
    Button.innerHTML = Content;
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

//Handle the Modal when it is hidden => an event will be raised and the data will be cleared
$(document).ready(function ()
{
    //Modal event when it is hidden it will be raised.
    $('.modal').on('hidden.bs.modal', function (e)
    {
        document.getElementById('ConfirmationResult').textContent = '';
        document.getElementById('CodeTextBox').value = '';
    });
});

//When the document (page) is ready and loaded, it will do the following:
//1. It will attempt to clear the sign up form.
//2. It will attempt to hide the modal.
$(document).ready(function ()
{
    ResetSignUp();
    HideModal('#ConfirmationModal');
});

//Handle the Sign Up Register Button POST's request using AJAX
$(document).ready(function ()
{
    $("#SignUpButton").on('click', e =>
    {
        e.preventDefault();

        let SignUpResultButton = document.getElementById('SignUpResult');
        SignUpResultButton.textContent = '';

        $("#SignUpForm").validate();

        if ($("#SignUpForm").valid())
        {
            ShowSpinner("SignUpButton", "Register");
            $.ajax
            ({
                type: "POST",
                url: 'SignUp?',
                data: $("#SignUpForm").serialize(),
                dataType: "json",
                success: function (response)
                {
                    HideSpinner("SignUpButton", "Register");
                    if (response != null)
                    {
                        console.log(response);

                        if (response.state === 'Valid') {
                            $('#ConfirmationModal').modal('show');
                            SignUpResultButton.textContent = '';
                        }
                        else {
                            SignUpResultButton.textContent = 'The Account is already registered.';
                        }
                    }
                    else
                    {
                        SignUpResultButton.textContent = 'No response from the server.';
                    }
                }
            });
        }
        else
        {
            SignUpResultButton.textContent = 'Invalid data, please insert a valid data';
        } 
    });
});

//Handle the Sign Up Confirmation Button POST's request using AJAX
$(document).ready(function () {
    $("#SubmitCodeButton").on('click', e =>
    {
        e.preventDefault();
        ShowSpinner('SubmitCodeButton', 'Submit');
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
                        HideSpinner('SubmitCodeButton', 'Submit');
                        ConfirmationResult.classList.replace("text-success", "text-danger");
                        ConfirmationResult.textContent = 'Invalid Confirmation Code';
                    }
                    else
                    {
                        ConfirmationResult.classList.replace("text-danger", "text-success");
                        ConfirmationResult.textContent = 'Redirection..';
                        window.setTimeout(function ()
                        {
                            HideSpinner('SubmitCodeButton', 'Submit');
                            Redirect('/SignUp/Successful', 1000);
                            HideModal('#ConfirmationModal');
                        }, 2500);
                    }
                }
            }
        });
    });
});


//Handle the Login Button POST's request using AJAX
$(document).ready(function () {
    $("#LoginButton").on('click', e =>
    {
        e.preventDefault();

        let LoginResult = document.getElementById('LoginResult');
        LoginResult.textContent = '';

        $("#LoginForm").validate();

        if ($("#LoginForm").valid())
        {
            ShowSpinner("LoginButton", "Submit");
            $.ajax
                ({
                    type: "POST",
                    url: 'Login?',
                    data: $("#LoginForm").serialize(),
                    dataType: "json",
                    success: function (response)
                    {
                        HideSpinner("LoginButton", "Submit");
                        if (response != null)
                        {
                            console.log(response);

                            if (response.state === 'Valid') {
                                $('#ConfirmationModal').modal('show');
                                LoginResult.textContent = '';
                            }
                            else if (response.state === 'ErrorPassword') {
                                LoginResult.textContent = 'Invalid Password, please enter a valid password.';
                            }
                            else if (response.state === 'ErrorActive') {
                                LoginResult.textContent = 'You are already signed in.';
                            }
                            else {
                                LoginResult.textContent = 'The Account is not registered.';
                            }
                        }
                        else
                        {
                            LoginResult.textContent = 'No response from the server.';
                        }
                    }
                });
        }
        else
        {
            LoginResult.textContent = 'Invalid data, please insert a valid data';
        }
    });
});

//Handle the login Confirmation Button POST's request using AJAX
$(document).ready(function ()
{
    $("#LoginCodeButton").on('click', e =>
    {
        e.preventDefault();
        ShowSpinner('LoginCodeButton', 'Submit');
        $.ajax
        ({
            type: "POST",
            url: 'Login/Check',
            data: $("#LoginForm").serialize(),
            dataType: "json",
            success: function (response)
            {
                if (response != null)
                {
                    console.log(response);
                    let ConfirmationResult = document.getElementById('ConfirmationResult');
                    if (response.state === 'Failed')
                    {
                        HideSpinner('LoginCodeButton', 'Submit');
                        ConfirmationResult.classList.replace("text-success", "text-danger");
                        ConfirmationResult.textContent = 'Invalid Confirmation Code';
                    }
                    else
                    {
                        ConfirmationResult.classList.replace("text-danger", "text-success");
                        ConfirmationResult.textContent = 'Redirection..';
                        window.setTimeout(function ()
                        {
                            HideSpinner('LoginCodeButton', 'Submit');
                            Redirect('/', 500);
                            HideModal('#ConfirmationModal');
                        }, 2500);
                    }
                }
            }
        });
    });
});