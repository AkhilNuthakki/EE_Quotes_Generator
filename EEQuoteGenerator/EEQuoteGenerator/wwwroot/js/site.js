// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

//validation for the password and confirm password match
var registerform = document.getElementById("registerForm");
if (registerform) {
    registerform.addEventListener("submit", e => {
        var password = document.getElementById("password").value;
        var confirmpassword = document.getElementById("confirmpassword").value;
        if (password != confirmpassword) {
            e.preventDefault();
            document.getElementById("confirmpassworderror").innerHTML = "Password doesn't match";
        }
    });
}


var clearButton = document.getElementById("clearBtn");

if (clearButton) {
    clearButton.addEventListener("click", e => {
        document.getElementById("QuoteReference").value = null;
        document.getElementById("CustomerEmail").value = null;
        document.getElementById("CustomerPostcode").value = null;

        location.href = '@Url.Action("Index", "History")';
    })
}


//validation for the finance details, if any of the three fields are non zero, all the fields should be non-zero to calculate finance model
var inputParametersForm = document.getElementById("inputParametersForm");

if (inputParametersForm) {
    inputParametersForm.addEventListener("submit", e => {

        var FieldsValidation = true;

        var BorrowingCost = parseFloat(document.getElementById("BorrowingCost").value);
        var PercentageFinanced = parseFloat(document.getElementById("PercentageFinanced").value);
        var LoanPeriodYears = parseFloat(document.getElementById("LoanPeriodYears").value);
         
        if (BorrowingCost > 0 || PercentageFinanced > 0 || LoanPeriodYears > 0) { 
            if (BorrowingCost <= 0 || PercentageFinanced <= 0 || LoanPeriodYears <= 0) {
                if (BorrowingCost <= 0) {
                    FieldsValidation = false;
                    document.getElementById("BorrowingCostError").innerHTML = "Borrowing cost should be greater than zero";
                }
                if (PercentageFinanced <= 0) {
                    FieldsValidation = false;
                    document.getElementById("PercentageFinancedError").innerHTML = "Percentage financed should be greater than zero";
                }
                if (LoanPeriodYears <= 0) {
                    FieldsValidation = false;
                    document.getElementById("LoanPeriodYearsError").innerHTML = "Loan period should be greater than zero";
                }
                
            }
        }

        if (!FieldsValidation) {
            e.preventDefault();
        }
    });
}

// an ajax call to email quote to customer
function EmailQuote(_id) {
    document.getElementById('StatusDiv').classList.remove('text-success');
    document.getElementById('StatusDiv').classList.remove('text-danger');
    document.getElementById('StatusMessage').innerText = 'Generating quote. It might take up to 20 seconds';
    document.getElementById('StatusDiv').classList.remove('d-none');
    document.getElementById('loadingIcon').classList.remove('loading-icon-invisible');
    document.getElementById('loadingIcon').classList.add('loading-icon-visible');
    $.ajax({
        url: EmailQuoteUrl,
        type: 'GET',
        data: { id: _id },
        contentType: 'application/json; charset=utf-8',
        datatype: 'json',
        success: function (result) {
            if (result.message == 'SUCCESS') {
                document.getElementById('StatusMessage').innerText = 'Email sent successfully !!';
                document.getElementById('StatusDiv').classList.remove('text-danger');
                document.getElementById('StatusDiv').classList.add('text-success');
                document.getElementById('loadingIcon').classList.remove('loading-icon-visible');
                document.getElementById('loadingIcon').classList.add('loading-icon-invisible');
            } else {
                document.getElementById('StatusMessage').innerText = 'Email sent failed !!';
                document.getElementById('StatusDiv').classList.remove('text-success');
                document.getElementById('StatusDiv').classList.add('text-danger');
                document.getElementById('loadingIcon').classList.remove('loading-icon-visible');
                document.getElementById('loadingIcon').classList.add('loading-icon-invisible');
            }
        },
        error: function (result) {
            document.getElementById('StatusMessage').innerText = 'Email sent failed !!';
            document.getElementById('StatusDiv').classList.remove('text-success');
            document.getElementById('StatusDiv').classList.add('text-danger');
            document.getElementById('loadingIcon').classList.remove('loading-icon-visible');
            document.getElementById('loadingIcon').classList.add('loading-icon-invisible');
        }
    })
}


//button click to show table and hide the graph
var tableButton = document.getElementById("tableBtn");

if (tableButton) {
    tableButton.addEventListener("click", e => {
        document.getElementById('regionQuotesTable').classList.remove('d-lg-none');
        document.getElementById('barchart_div').classList.remove('d-lg-flex');
        document.getElementById("tableBtn").classList.add('icon-clicked');
        document.getElementById("graphBtn").classList.remove('icon-clicked');
    })
}

//button click to show the graph and hide the table
var graphButton = document.getElementById("graphBtn");

if (graphButton) {
    graphButton.addEventListener("click", e => {
        document.getElementById('regionQuotesTable').classList.add('d-lg-none');
        document.getElementById('barchart_div').classList.add('d-lg-flex');
        document.getElementById("tableBtn").classList.remove('icon-clicked');
        document.getElementById("graphBtn").classList.add('icon-clicked');
    })
}