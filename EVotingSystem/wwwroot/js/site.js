//Helper Function takes a character parameter and returns
//True: if the character is a digit between 0 and 9
//False: if the character is a non digit.
function IsDigit(c) {
    return (c >= '0' && c <= '9');
}

//Helper Function takes a character parameter and returns
//True: if the character is a letter betweeen A-Z/a-z
//False: if the character is a non letter.
function IsLetter(c) {
    return ((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z'));
}

//Helper Function takes two parameter, first the path represents the rout
//second is interval represents delay in milliseconds, 1000ms = 1s.
function Redirect(Path, Interval) {
    window.setTimeout(function () {
        window.location.href = Path;
    }, Interval);
}

//Helper Function takes one parameter that represents the Id of the modal.
//The function hides the modal.
function HideModal(Id) {
    $(Id).modal('hide');
}

//Helper Function that resets all of the inputs from the sign up
function ResetSignUp() {
    let SignUpForm = document.getElementById('SignUpForm');
    if (SignUpForm != null) {
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
function ShowSpinner(Id, Content) {
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
function HideSpinner(Id, Content) {
    let Button = document.getElementById(Id);
    //Enable Button
    Button.removeAttribute("disabled");
    //Remove all of the Injected Spinner Code and Add a new Text.
    Button.innerHTML = Content;
}

function AddCandidateRow(Name, Gender, Id) {

    let Count = $('#CandidateTable tr').length + 1;
    $('#CandidateTable > tbody:last-child').append(
        "<tr class=\"text-dark\" id=\"" + Id + "\">" +
        "<th scope=\"row\">" + Count + "</th>" +
        "<td>" + Id + "</td>" +
        "<td>" + Name + "</td>" +
        "<td>" + Gender + "</td>" +
        "<td>" + 0 + "</td>" +
        "<td>" +
        "<button type=\"button\" class=\"close\" onClick=\"DeleteCandidateConfirmation('" + Id + "');\">" +
        "<span aria-hidden=\"true\" class=\"text-danger\">&times;</span>" +
        "</button>" +
        "</td>" +
        "</tr>");
}

//Add the Name Validation that is hooked with the server side validation.
jQuery.validator.addMethod('NameValidation', function (value, element) {
    if (value == null || value.length == 0) {
        return false;
    }
    else {
        for (var i = 0; i < value.length; i++) {
            if (IsLetter(value[i]) == false) {
                return false;
            }
        }
    }
    return true;
});
$.validator.unobtrusive.adapters.addBool("NameValidation");


//Add the StudentId Validation that is hooked with the server side validation.
jQuery.validator.addMethod('StudentIdValidation', function (value, element) {
    if (value == null || value.length != 9) {
        return false;
    }
    else {
        if ((value[0] == 'H') || (value[0] == 'h')) {
            for (var i = 1; i < value.length; i++) {
                if (IsDigit(value[i]) == false) {
                    return false;
                }
            }
        }
        else {
            return false;
        }
    }
    return true;
});
$.validator.unobtrusive.adapters.addBool("StudentIdValidation");

//Add the EmailValidation that is hooked with the server side validation.
jQuery.validator.addMethod('EmailValidation', function (value, element) {
    const Domain = "hct.ac.ae";
    const IdLength = 9;

    if (value == null || value.includes("@") == false) {
        return false;
    }
    else {
        const Parts = value.split("@");
        if (Parts.length != 2) {
            return false;
        }
        else {
            let CurrentId = Parts[0];
            let CurrentDomain = Parts[1];

            if (CurrentDomain != Domain || CurrentId.length != IdLength) {
                return false;
            }
            else {
                if (CurrentId[0] != 'H' && CurrentId[0] != 'h') {
                    return false;
                }
                else {
                    for (var i = 1; i < CurrentId.length; i++) {
                        if (IsDigit(CurrentId[i]) == false) {
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

//Add the GenderValidation that is hooked with the server side validation.
jQuery.validator.addMethod('GenderValidation', function (value, element) {

    let Gender = String(value).toLowerCase();
    if (Gender == null || (Gender != "male" && Gender != "female")) {
        return false;
    }
    return true;
});
$.validator.unobtrusive.adapters.addBool("GenderValidation");

//Add the NationalIdValidation that is hooked with the server side validation.
jQuery.validator.addMethod('NationalIdValidation', function (value, element) {

    let NationalId = String(value);
    if (NationalId == null || NationalId.length != 18 || NationalId.includes("-") == false) {
        return false;
    }
    else {
        const Parts = NationalId.split("-");
        if (Parts.length != 4) {
            return false;
        }
        else {
            if (Parts[0].length != 3 || Parts[1].length != 4 || Parts[2].length != 7 || Parts[3].length != 1) {
                return false;
            }
            else {
                for (var i = 0; i < 4; i++) {
                    for (var j = 0; j < Parts[i].length; j++) {
                        if (IsDigit(Parts[i][j]) == false) {
                            return false;
                        }
                    }
                }
            }
        }
    }
    return true;
});
$.validator.unobtrusive.adapters.addBool("NationalIdValidation");

//Handle the "Back to main page" Button
$(document).ready(function () {
    $('#BackToMainButton').on('click', e => {
        Redirect('/', 1000);
    });
});

//Handle the Modal when it is hidden => an event will be raised and the data will be cleared
$(document).ready(function () {
    //Modal event when it is hidden it will be raised.
    $('#ConfirmationModal').on('hidden.bs.modal', function (e) {
        document.getElementById('ConfirmationResult').textContent = '';
        document.getElementById('CodeTextBox').value = '';
    });
    //Modal event when it is hidden it will be raised.
    $('#DeleteStudentConfirmation').on('hidden.bs.modal', function (e) {
        document.getElementById('DeleteStudentResult').textContent = '';
    });
    //Modal event when it is hidden it will be raised.
    $('#DeleteCandidateConfirmation').on('hidden.bs.modal', function (e) {
        document.getElementById('DeleteCandidateResult').textContent = '';
    });
    //Modal even when it is hidden it will be raised.
    $('#AddCandidateModal').on('hidden.bs.modal', function (e) {
        document.getElementById('CNameTB').value    = '';
        document.getElementById('CGenderTB').value  = '';
        document.getElementById('CIdTB').value      = '';
        document.getElementById('CSpeachTB').value  = '';
        document.getElementById('CImageTB').value = '';
        document.getElementById('AddCandidateResult').textContent = '';
    });
});

//When the document (page) is ready and loaded, it will do the following:
//1. It will attempt to clear the sign up form.
//2. It will attempt to hide the modals.
$(document).ready(function () {
    ResetSignUp();
    HideModal('#ConfirmationModal');
    HideModal('#DeleteStudentConfirmation');
    HideModal('#DeleteCandidateConfirmation');
});

//When the document (page) is ready and loaded, it will do the following:
//1. Handle the Vote button click
//2. If the user clicks "YES" it will attempt the vote, otherwise it will just close the modal.
$(document).ready(function () {
    $("#VoteButton").on('click', e => {
        console.log("VoteButton Clicked");
        $('#VoteConfirmationModal').modal('show');
    });
});

//Handle the Sign Up Register Button POST's request using AJAX
$(document).ready(function () {
    $("#SignUpButton").on('click', e => {
        e.preventDefault();

        let SignUpResultButton = document.getElementById('SignUpResult');
        SignUpResultButton.textContent = '';

        $("#SignUpForm").validate();

        if ($("#SignUpForm").valid()) {
            ShowSpinner("SignUpButton", "Register");
            $.ajax
                ({
                    type: "POST",
                    url: 'SignUp?',
                    data: $("#SignUpForm").serialize(),
                    dataType: "json",
                    success: function (response) {
                        HideSpinner("SignUpButton", "Register");
                        if (response != null) {
                            console.log(response);

                            if (response.state === 'Valid') {
                                $('#ConfirmationModal').modal('show');
                                SignUpResultButton.textContent = '';
                            }
                            else {
                                SignUpResultButton.textContent = 'The Account is already registered.';
                            }
                        }
                        else {
                            SignUpResultButton.textContent = 'No response from the server.';
                        }
                    }
                });
        }
        else {
            SignUpResultButton.textContent = 'Invalid data, please insert a valid data';
        }
    });
});

//Handle the Sign Up Confirmation Button POST's request using AJAX
$(document).ready(function () {
    $("#SubmitCodeButton").on('click', e => {
        e.preventDefault();
        ShowSpinner('SubmitCodeButton', 'Submit');
        $.ajax
            ({
                type: "POST",
                url: 'SignUp/Check',
                data: $("#SignUpForm").serialize(),
                dataType: "json",
                success: function (response) {
                    if (response != null) {
                        console.log(response);
                        let ConfirmationResult = document.getElementById('ConfirmationResult');
                        if (response.state === 'Failed') {
                            HideSpinner('SubmitCodeButton', 'Submit');
                            ConfirmationResult.classList.replace("text-success", "text-danger");
                            ConfirmationResult.textContent = 'Invalid Confirmation Code';
                        }
                        else {
                            ConfirmationResult.classList.replace("text-danger", "text-success");
                            ConfirmationResult.textContent = 'Redirection..';
                            window.setTimeout(function () {
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

//Handles the Account type chosen by the client
$(document).ready(function () {
    $('#AccountTypeComboBox').on('change', function () {
        document.getElementById("StudentIdTextBox").value = "";
        document.getElementById("AdminEmailTextBox").value = "";
        if ($(this).val() == "Student") {
            if ($("#AdminEmailRow").hasClass("HideToken") == false) {
                //Hide Admin Email and Remove Validation Attributes
                $("#AdminEmailRow").addClass("HideToken");
                $("#AdminEmailTextBox").removeAttr('data-val-required');
                $("#AdminEmailTextBox").removeAttr('data-val-maxlength');
            }
            //Add Validation Attributes to the Student Id
            $("#StudentIdTextBox").attr('data-val-required', 'Please insert your Student Id');
            $("#StudentIdTextBox").attr('data-val-maxlength', 'Student Id cannot exceed more than 9 characters');
            $("#StudentIdTextBox").attr('data-val-studentidvalidation', 'Invalid Student Id, please insert a valid HCT Id');

            //Remove Hidden Tags from Recovery Link and Student Id
            $('#RecoverLink').removeClass("HideToken");
            $("#StudentIdRow").removeClass("HideToken");
            $("#StudentIdRow").show();
        }
        else if ($(this).val() == "Administrator") {
            if ($("#StudentIdRow").hasClass("HideToken") == false) {
                //Hide Student Id and Remove Validation Attributes
                $("#StudentIdRow").addClass("HideToken");
                $('#RecoverLink').addClass("HideToken");
                $("#StudentIdTextBox").removeAttr('data-val-required');
                $("#StudentIdTextBox").removeAttr('data-val-maxlength');
                $("#StudentIdTextBox").removeAttr('data-val-studentidvalidation');
            }
            //Add Validation Attributes to the Admin Email
            $("#AdminEmailTextBox").attr('data-val-required', 'Please insert your Email');
            $("#AdminEmailTextBox").attr('data-val-maxlength', 'Email cannot exceed more than 30 characters');
            //Remove Hidden Tags from Admin Email
            $("#AdminEmailRow").removeClass("HideToken");
            $("#AdminEmailRow").show();
        }
    });
});

//Handle the Login Button POST's request using AJAX
$(document).ready(function () {
    $("#LoginButton").on('click', e => {
        e.preventDefault();

        let LoginResult = document.getElementById('LoginResult');
        LoginResult.textContent = '';

        $("#LoginForm").validate();

        if ($("#LoginForm").valid()) {
            ShowSpinner("LoginButton", "Submit");
            $.ajax
                ({
                    type: "POST",
                    url: 'Login?',
                    data: $("#LoginForm").serialize(),
                    dataType: "json",
                    success: function (response) {

                        if (response != null) {
                            console.log(response);

                            if (response.state === 'Valid') {
                                $('#ConfirmationModal').modal('show');
                                LoginResult.textContent = '';
                                HideSpinner("LoginButton", "Submit");
                            }
                            else if (response.state === 'ErrorPassword') {
                                LoginResult.textContent = 'Invalid Password, please enter a valid password.';
                                HideSpinner("LoginButton", "Submit");
                            }
                            else if (response.state === 'ErrorActive') {
                                LoginResult.textContent = 'You are already signed in.';
                                HideSpinner("LoginButton", "Submit");
                            }
                            else if (response.state === 'Success') {
                                ConfirmationResult.classList.replace("text-danger", "text-success");
                                ConfirmationResult.textContent = 'Redirection..';
                                window.setTimeout(function () {
                                    HideSpinner("LoginButton", "Submit");
                                    Redirect('/', 500);
                                }, 2500);
                            }
                            else {
                                LoginResult.textContent = 'The Account is not registered.';
                                HideSpinner("LoginButton", "Submit");
                            }
                        }
                        else {
                            LoginResult.textContent = 'No response from the server.';
                            HideSpinner("LoginButton", "Submit");
                        }
                    }
                });
        }
        else {
            LoginResult.textContent = 'Invalid data, please insert a valid data';

        }
    });
});

//Handle the login Confirmation Button POST's request using AJAX
$(document).ready(function () {
    $("#LoginCodeButton").on('click', e => {
        e.preventDefault();
        ShowSpinner('LoginCodeButton', 'Submit');
        $.ajax
            ({
                type: "POST",
                url: 'Login/Check',
                data: $("#LoginForm").serialize(),
                dataType: "json",
                success: function (response) {
                    if (response != null) {
                        console.log(response);
                        let ConfirmationResult = document.getElementById('ConfirmationResult');
                        if (response.state === 'Failed') {
                            HideSpinner('LoginCodeButton', 'Submit');
                            ConfirmationResult.classList.replace("text-success", "text-danger");
                            ConfirmationResult.textContent = 'Invalid Confirmation Code';
                        }
                        else {
                            ConfirmationResult.classList.replace("text-danger", "text-success");
                            ConfirmationResult.textContent = 'Redirection..';
                            window.setTimeout(function () {
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

//Search Functionality for Student
$(document).ready(function () {
    var $rows = $('#StudentTable tr');
    $('#StudentSearch').keyup(function () {
        var val = $.trim($(this).val()).replace(/ +/g, ' ').toLowerCase();

        $rows.show().filter(function () {
            var text = $(this).text().replace(/\s+/g, ' ').toLowerCase();
            return !~text.indexOf(val);
        }).hide();
    });
});

//Search Functionality for Candidate
$(document).ready(function () {
    var $rows = $('#CandidateTable tr');
    $('#CandidateSearch').keyup(function () {
        var val = $.trim($(this).val()).replace(/ +/g, ' ').toLowerCase();

        $rows.show().filter(function () {
            var text = $(this).text().replace(/\s+/g, ' ').toLowerCase();
            return !~text.indexOf(val);
        }).hide();
    });
});

//Handles the deletion of the student in admin dashboard.
function DeleteStudentConfirmation(Id) {
    console.log("Delete Student Confirmation: " + Id);
    document.getElementById('DeleteStudentButton').onclick = function () { DeleteStudent(Id); };
    $('#DeleteStudentConfirmation').modal('show');
}
function DeleteStudent(Id) {
    console.log("Delete Student On Click call back: " + Id);
    ShowSpinner('DeleteStudentButton', 'Delete');
    $.ajax
        ({
            type: "POST",
            url: 'Dashboard/DeleteStudent',
            data: { Id: Id },
            dataType: "json",
            success: function (response) {
                HideSpinner('DeleteStudentButton', 'Delete');
                let DeleteStudentResult = document.getElementById('DeleteStudentResult');
                if (response.state === 'Success') {
                    DeleteStudentResult.textContent = 'Successfully deleted';
                    HideModal('#DeleteStudentConfirmation');
                    document.getElementById(Id).remove();
                }
                else {
                    DeleteStudentResult.classList.replace("text-success", "text-danger");
                    DeleteStudentResult.textContent = 'Failed to delete';
                }
            }
        });
}

function DeleteCandidateConfirmation(Id) {
    console.log("Delete Candidate Confirmation: " + Id);
    document.getElementById('DeleteCandidateButton').onclick = function () { DeleteCandidate(Id); };
    $('#DeleteCandidateConfirmation').modal('show');
}

//Handles the deletion of the candidate in admin dashboard.
function DeleteCandidate(Id) {
    console.log("Delete Candidate Confirmed: " + Id);
    ShowSpinner('DeleteCandidateButton', 'Delete');
    $.ajax
        ({
            type: "POST",
            url: 'Dashboard/DeleteCandidate',
            data: { Id: Id },
            dataType: "json",
            success: function (response) {
                HideSpinner('DeleteCandidateButton', 'Delete');
                let DeleteCandidateResult = document.getElementById('DeleteCandidateResult');
                if (response.state === 'Success') {
                    DeleteCandidateResult.textContent = 'Successfully deleted';
                    HideModal('#DeleteCandidateConfirmation');
                    document.getElementById(Id).remove();
                }
                else {
                    DeleteCandidateResult.classList.replace("text-success", "text-danger");
                    DeleteCandidateResult.textContent = 'Failed to delete';
                }
            }
        });
}


$(document).ready(function () {
    $('#OpenAddCandidateModal').on('click', e => {
        $('#AddCandidateModal').modal('show');
    });

    $('#AddCandidateButton').on('click', e => {
        ShowSpinner('AddCandidateButton', 'Add Candidate');
        $.ajax
            ({
                type: "POST",
                url: 'Dashboard/AddCandidate',
                data: { Candidate: "".concat($('#CNameTB').val(), "|", $('#CGenderTB').val(), "|", $('#CIdTB').val(), "|", $('#CSpeachTB').val(), "|", $('#CImageTB').val()) },
                dataType: "json",
                success: function (response) {
                    console.log("Add Candidate Response: " + response.state);

                    HideSpinner('AddCandidateButton', 'Add Candidate');
                    let AddCandidateResult = document.getElementById('AddCandidateResult');
                    if (response.state === 'Success') {
                        AddCandidateResult.textContent = 'Successfully added';
                        HideModal('#AddCandidateModal');
                        AddCandidateRow(response.name, response.gender, response.id);
                    }
                    else {
                        AddCandidateResult.classList.replace("text-success", "text-danger");
                        AddCandidateResult.textContent = response.reason;
                    }
                }
            });
    });
});


//Handle the Contact Button POST's request using AJAX
$(document).ready(function () {
    $("#ContactButton").on('click', e =>
    {
        e.preventDefault();
        $("#ContactForm").validate();
        if ($("#ContactForm").valid())
        {
            let ContactResult = document.getElementById('ContactResult');
            ShowSpinner("ContactButton", "Send");
            $.ajax
            ({
                type: "POST",
                url: 'Contact',
                data: $("#ContactForm").serialize(),
                dataType: "json",
                success: function (response)
                {
                    console.log("Response: " + response.state)
                    if (response.state == 'Success')
                    {
                        window.setTimeout(function ()
                        {
                            HideSpinner("ContactButton", "Send");
                            Redirect('/Contact/Success', 100);
                        }, 2500);
                    }
                    else
                    {
                        HideSpinner("ContactButton", "Send");
                        ContactResult.textContent = "Failed to send a message, please try again later.";
                    }
                }
            });
        }
    });
});

//Handle Adding Voting Due Date Button POST's request using AJAX
function InsertDueDate()
{
    let VotingConfigResult = document.getElementById('VotingConfigResult');
    let DueDateValue = $('#DueDateValue').val();
    console.log("DueDateValue: " + DueDateValue);
    $.ajax
    ({
        type: "POST",
        url: 'Dashboard/AddDueDate',
        data: { DueDate: DueDateValue},
        dataType: "json",
        success: function (response)
        {
            console.log("Response: " + response.state)
            if (response.state == 'Success')
            {
                $("#InsertDueDateButton").addClass("disabled");
                $("#DeleteDueDateButton").removeClass("disabled");
                VotingConfigResult.textContent = "Successfully added, the due date is set to: " + response.dueDate + ".";
            }
            else if (response.state == 'Invalid')
            {
                VotingConfigResult.textContent = "Invalid Due date, should be after current date.";
            }
            else
            {
                VotingConfigResult.textContent = "Failed to set due date, please try again later.";
            }
        }
    });
}

//Handle Deleting Voting Due Date Button POST's request using AJAX
function DeleteDueDate()
{
    let VotingConfigResult = document.getElementById('VotingConfigResult');
    $.ajax
    ({
        type: "POST",
        url: 'Dashboard/DeleteDueDate',
        dataType: "json",
        success: function (response)
        {
            console.log("Response DeleteDueDate: " + response.state)
            if (response.state == 'Success')
            {
                $("#DeleteDueDateButton").addClass("disabled");
                $("#InsertDueDateButton").removeClass("disabled");
                VotingConfigResult.textContent = "Successfully deleted.";
                $('#DueDateValue').val('');
            }
            else
            {
                VotingConfigResult.textContent = "Failed to delete.";
            }
        }
    });
}


//Handle Result Count down
$(document).ready(function () {

    function getRemainingDays() {
        return Date.parse($("#SpanDueDate").text());
    }
    function TestCountdown() {
        return new Date(new Date().valueOf() + 60 * 60);
    }
    $('#clock-c').countdown(TestCountdown(), function (event) {
        var $this = $(this).html(event.strftime(''
            + '<span class="h1 font-weight-bold">%D</span> Day%!d'
            + '<span class="h1 font-weight-bold">%H</span> Hr'
            + '<span class="h1 font-weight-bold">%M</span> Min'
            + '<span class="h1 font-weight-bold">%S</span> Sec'));
    }).on('finish.countdown', function ()
    {
        console.log("Count down finished, attemtping to make ajax request");
        $.ajax
        ({
            type: "POST",
            url: 'Results/Check',
            dataType: "json",
            success: function (response)
            {
                console.log("Response from Check: " + response.state)
                if (response.state == 'Success')
                {
                    console.log("Winner Candidate is: " + response.id);
                    Redirect("/Results", 1000);
                }
                else
                {
                    console.log("Failed to get the winner candidate. Reason: " + response.reason);
                }
            }
        });
    });

    $('#btn-pause').click(function () {
        $('#clock-c').countdown('pause');
    });
    $('#btn-resume').click(function () {
        $('#clock-c').countdown('resume');
    });
});