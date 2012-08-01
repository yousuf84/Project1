
var StudentDetailDisplay = function () {
    this.StudentID;
    this.FirstName;
    this.LastName;
    this.FatherName;
    this.SchoolName;
    this.DateOfBirth;
    this.Mobile;
    this.EmailID;

};

$(document).ready(function () {

    $('#DateOfBirth').datepicker({ dateFormat: "dd/mm/yy" });

    // var studentdetails = JSON.parse($("#StudentDetailsData").text());


    var studentdetaildisplay = new StudentDetailDisplay();

    //        studentdetaildisplay.StudentID = ko.observable(studentdetails.StudentID);
    //        studentdetaildisplay.FirstName = ko.observable(studentdetails.FirstName);
    //        studentdetaildisplay.LastName = ko.observable(studentdetails.LastName);
    //        studentdetaildisplay.FatherName = ko.observable(studentdetails.FatherName);
    //        studentdetaildisplay.SchoolName = ko.observable(studentdetails.SchoolName);
    //        studentdetaildisplay.DateOfBirth = ko.observable(studentdetails.DateOfBirth);
    //        studentdetaildisplay.Mobile = ko.observable(studentdetails.Mobile);
    //        studentdetaildisplay.EmailID = ko.observable(studentdetails.EmailID);

    //        studentdetaildisplay = ko.observable();
    //        ko.applyBindings(studentdetaildisplay, ".container");
    $('#contact-form').validate({
        rules: {
            FirstName: {
                required: true
            },

            LastName: {
                required: true
            },

            FatherName: {
                required: true
            },

            SchoolName: {
                required: true
            },

            DateOfBirth: {
                required: true,
                date: true
            },

            EmailID: {
                required: true,
                email: true
            },

            Mobile: {
                required: true
            }
        },
        highlight: function (label) {
            $(label).closest('.control-group').addClass('error');
        },
        success: function (label) {
            label
	    		.text('OK!').addClass('valid')
	    		.closest('.control-group').addClass('success');
        }
    });

});   // end document.ready