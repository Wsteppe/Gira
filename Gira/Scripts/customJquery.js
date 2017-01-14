$(document).ready(function () {
    $("#copyrightSpan").fadeIn(3500);

    //dirty code, but good enough for non-production code
    $("#adminDropDown").click(function () {
        $("#adminIssues").slideToggle();
    });

    $("#managedDropDown").click(function () {
        $("#managedIssues").slideToggle();
    });

    $("#responsibleDropDown").click(function () {
        $("#responsibleIssues").slideToggle();
    });

    $("#createdDropDown").click(function () {
        $("#createdIssues").slideToggle();
    });
});