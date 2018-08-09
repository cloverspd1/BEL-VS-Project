var GetFinalizationReportExportToExcelUrl = BASEPATHURL + "/Feedbacks/GetFeedbackReportExportToExcel";
$(document).ready(function () {
    $(".sectionDetailType").change();

    BindUserTags("");
    //if ($("#AttachFile").length > 0) {
    if ($("#FileNameList").length != 0) {
        BindFileUploadControl({
            ElementId: 'AttachFile', Params: {}, Url: "UploadFile",
            AllowedExtensions: [],
            MultipleFiles: false,
            CallBack: "OnFileUploaded"
        });
        uploadedFiles = BindFileList("FileNameList", "AttachFile");
    }
    if ($("#CCFileNameList").length != 0) {
        BindFileUploadControl({
            ElementId: 'CCAttachFile', Params: {}, Url: "UploadFile",
            AllowedExtensions: [],
            MultipleFiles: false,
            CallBack: "OnCCFileUploaded"
        });
        ccuploadedFiles = BindFileList("CCFileNameList", "CCAttachFile");
    }
    if ($("#CCQAInchargeFileNameList").length != 0) {
        BindFileUploadControl({
            ElementId: 'CCQAAttachFile', Params: {}, Url: "UploadFile",
            AllowedExtensions: [],
            MultipleFiles: false,
            CallBack: "OnCCQAFileUploaded"
        });

        ccqauploadedFiles = BindFileList("CCQAInchargeFileNameList", "CCQAAttachFile");
    }
    $("input[name='ForwardtoCCQualityIncharge']").on("change", function () {

        if ($("#WorkflowStatus") !== undefined && $("#WorkflowStatus").val().indexOf("Quality") == -1) {
            if ($('#ForwardtoQualityIncharge1')[0].checked == true) {
                $('.CCQualityInchargeUser').removeClass("hide");
                var btntext = $("label[for='ForwardtoQualityIncharge2']").text();
                $("a.btn:contains('Send to CC Quality Incharge User')").text($("label[for='ForwardtoQualityIncharge1']").text());
                $("a.btn:contains('" + btntext + "')").text($("label[for='ForwardtoQualityIncharge1']").text());
                $("a.btn:contains('Forward to CC Quality Incharge')").attr('data-original-title', "Request will move to CC Quality Incharge.");
            }
            else {
                var btntext = $("label[for='ForwardtoQualityIncharge1']").text();
                $("a.btn:contains('Send to CC Quality Incharge User')").text($("label[for='ForwardtoQualityIncharge2']").text());
                $("a.btn:contains('" + btntext + "')").text($("label[for='ForwardtoQualityIncharge2']").text());

                $('.CCQualityInchargeUser').addClass("hide");

                $("a.btn:contains('Close the issue')").attr('data-original-title', "Request will be closed.");
            }
        }

    }).change();

    $("input[name='ForwardtoQuality']").on("change", function () {
        if ($("#WorkflowStatus") !== undefined && $("#WorkflowStatus").val().indexOf("Quality") >= 0) {
            if ($('#ForwardtoQuality1')[0].checked == true) {
                $('.qauser').removeClass("hide");
                var btntext = $("label[for='ForwardtoQuality2']").text();
                $("a.btn:contains('Send to QA User')").text($("label[for='ForwardtoQuality1']").text());
                $("a.btn:contains('" + btntext + "')").text($("label[for='ForwardtoQuality1']").text());
                $("a.btn:contains('Forward to Quality')").attr('data-original-title', "Request will move to Quality User.");
            }
            else {
                var btntext = $("label[for='ForwardtoQuality1']").text();
                $("a.btn:contains('Send to QA User')").text($("label[for='ForwardtoQuality2']").text());
                $("a.btn:contains('" + btntext + "')").text($("label[for='ForwardtoQuality2']").text());

                $('.qauser').addClass("hide");
                $("a.btn:contains('Close the issue')").attr('data-original-title', "Request will be closed.");
            }
        }
    }).change();

    
    $("#SendBackToCC").click(function () {
       
        if ($(this).is(":checked")) {
            $("#closediv").hide();
            $("#quastatusdiv").show();
            $("#quastatusdiv").css("visibility", "visible");
            $("a.btn:contains('Complete')").text("Send Back");
            $("a.btn:contains('Send Back')").attr('data-original-title', "Request will be forward to CC.");
            $("a.btn:contains('Send Back')").attr('data-action', "22");
           
        } else {
            $("#closediv").show();
            $("#quastatusdiv").hide();
            $("#quastatusdiv").css("visibility", "hidden");
            $("a.btn:contains('Send Back')").text("Complete");
            $("a.btn:contains('Complete')").attr('data-original-title', "Request will be close.");
            $("a.btn:contains('Complete')").attr('data-action', "14");
        }
    });

    //if ($("#sendbacktocc").is(":checked")) {
    //    $("#qualitystatus").rules("add", "required");
    //    $("#observations").rules("remove", "required");
    //    $("#actionplans").rules("remove", "required");
    //    $("#problemcause").rules("remove", "required");
       
    //} else {
    //    $("#qualitystatus").rules("remove", "required");
    //    $("#observations").rules("add", "required");
    //    $("#actionplans").rules("add", "required");
    //    $("#problemcause").rules("add", "required");
    //    $("#quastatusdiv").addclass("hide");
    //}
    
    $("#QualityStatus").on("change", function () {
        var txt = $(this).val();
        if (txt != "") {
            var btntext = "Forward to CC";
            if (txt == "Others") {
               
                $(".quacomments").removeClass("hide");
            }
            else {
                $(".quacomments").addClass("hide");
            }
        }else {
            $(".quacomments").addClass("hide");
               
            }
    }).change();

   

   


    //For QA
    //$('#QAUserList').multiselect({
    //    onChange: function (option, checked) {
    //        // Get selected options.

    //        var selectedOptions = $('#QAUserList option:selected');

    //        $("input[type='hidden'][name='QAUser']").val($('select#QAUserList').val());
    //        $("input[type='hidden'][name='QAUserName']").val(GetMultiselectValue(selectedOptions));

    //        if (selectedOptions.length >= 3) {
    //            // Disable all other checkboxes.
    //            var nonSelectedOptions = $('#QAUserList option').filter(function () {
    //                return !$(this).is(':selected');
    //            });

    //            nonSelectedOptions.each(function () {
    //                var input = $('input[value="' + $(this).val() + '"]');
    //                input.prop('disabled', true);
    //                input.parent('li').addClass('disabled');
    //            });
    //        }
    //        else {
    //            // Enable all checkboxes.
    //            $('#QAUserList option').each(function () {
    //                var input = $('input[value="' + $(this).val() + '"]');
    //                input.prop('disabled', false);
    //                input.parent('li').addClass('disabled');
    //            });
    //        }
    //    }
    //});

    $('#QAUserList').multiselect({
        includeSelectAllOption: true
    });

    var selectedGender = $("#QAUserList").attr("data-selected");
    if ($.trim(selectedGender) != "") {
        $('#QAUserList').multiselect('select', selectedGender.split(","));
    }

    $("#QAUserList").on("change", function () {

        $('#QAUser').val($("#QAUserList").val());
        var selectedOptions = $('#QAUserList option:selected');
        $("input[type='hidden'][name='QAUserName']").val(GetMultiselectValue(selectedOptions));
    }).change();

    //if ($.trim($("#QAUserList").attr("data-selected")) != "") {
    //    $('#QAUserList').multiselect('select', $("#QAUserList").attr("data-selected").split(","));
    //}

   


    debugger;
    $("#wipDCRTable").DataTable({
        "paging": true,
        "searching": true,
        "info": false,
        "order": [[1, "asc"]],
        "fixedHeader": true
    });


    jQuery('#btnGenerateFinalizationReportExportToExcel').on('click', function () { GetFinalizationReportExportToExcel(); });

});

function GetFinalizationReportExportToExcel() {
    window.open(GetFinalizationReportExportToExcelUrl);
}
var uploadedFiles = [], ccuploadedFiles = [], ccqauploadedFiles = [];

function OnFileUploaded(result) {
    uploadedFiles.push(result);
    $("#FileNameList").val(JSON.stringify(uploadedFiles)).blur();
}

function AttachFileRemoveImage(ele) {
    var Id = $(ele).attr("data-id");
    var li = $(ele).parents("li.qq-upload-success");
    var itemIdx = li.index();
    ConfirmationDailog({
        title: "Remove", message: "Are you sure to remove file?", id: Id, url: "/Master/RemoveUploadFile", okCallback: function (id, data) {
            li.find(".qq-upload-status-text").remove();
            $('<span class="qq-upload-spinner"></span>').appendTo(li);
            li.removeClass("qq-upload-success");
            var idx = -1;
            var tmpList = [];
            $(uploadedFiles).each(function (i, item) {
                if (idx == -1 && item.FileId == id) {
                    idx = i;
                    if (item.Status == 0) {
                        item.Status = 2;
                        tmpList.push(item);
                    }
                } else {
                    tmpList.push(item);
                }
            });
            if (idx >= 0) {
                uploadedFiles = tmpList;
                li.remove();
                if (uploadedFiles.length == 0) {
                    $("#FileNameList").val("").blur();
                } else {
                    $("#FileNameList").val(JSON.stringify(uploadedFiles)).blur();
                }
            }
        }
    });
}

function OnCCFileUploaded(result) {
    ccuploadedFiles.push(result);
    $("#CCFileNameList").val(JSON.stringify(ccuploadedFiles)).blur();
}
function OnCCQAFileUploaded(result) {
    ccqauploadedFiles.push(result);
    $("#CCQAInchargeFileNameList").val(JSON.stringify(ccqauploadedFiles)).blur();
}

function CCAttachFileRemoveImage(ele) {
    var Id = $(ele).attr("data-id");
    var li = $(ele).parents("li.qq-upload-success");
    var itemIdx = li.index();
    ConfirmationDailog({
        title: "Remove", message: "Are you sure to remove file?", id: Id, url: "/Master/RemoveUploadFile", okCallback: function (id, data) {
            li.find(".qq-upload-status-text").remove();
            $('<span class="qq-upload-spinner"></span>').appendTo(li);
            li.removeClass("qq-upload-success");
            var idx = -1;
            var tmpList = [];
            $(ccuploadedFiles).each(function (i, item) {
                if (idx == -1 && item.FileId == id) {
                    idx = i;
                    if (item.Status == 0) {
                        item.Status = 2;
                        tmpList.push(item);
                    }
                } else {
                    tmpList.push(item);
                }
            });
            if (idx >= 0) {
                ccuploadedFiles = tmpList;
                li.remove();
                if (ccuploadedFiles.length == 0) {
                    $("#CCFileNameList").val("").blur();
                } else {
                    $("#CCFileNameList").val(JSON.stringify(ccuploadedFiles)).blur();
                }
            }
        }
    });
}
function CCQAAttachFileRemoveImage(ele) {
    var Id = $(ele).attr("data-id");
    var li = $(ele).parents("li.qq-upload-success");
    var itemIdx = li.index();
    ConfirmationDailog({
        title: "Remove", message: "Are you sure to remove file?", id: Id, url: "/Master/RemoveUploadFile", okCallback: function (id, data) {
            li.find(".qq-upload-status-text").remove();
            $('<span class="qq-upload-spinner"></span>').appendTo(li);
            li.removeClass("qq-upload-success");
            var idx = -1;
            var tmpList = [];
            $(ccqauploadedFiles).each(function (i, item) {
                if (idx == -1 && item.FileId == id) {
                    idx = i;
                    if (item.Status == 0) {
                        item.Status = 2;
                        tmpList.push(item);
                    }
                } else {
                    tmpList.push(item);
                }
            });
            if (idx >= 0) {
                ccqauploadedFiles = tmpList;
                li.remove();
                if (ccqauploadedFiles.length == 0) {
                    $("#CCQAInchargeFileNameList").val("").blur();
                } else {
                    $("#CCQAInchargeFileNameList").val(JSON.stringify(ccqauploadedFiles)).blur();
                }
            }
        }
    });
}

function ItemCodeAdded(ele, id, text) {

    ShowWaitDialog();
    AjaxCall({
        url: "/Feedbacks/GetSKUInfo?itemCode=" + id,
        httpmethod: "GET",
        sucesscallbackfunction: function (result) {
            var isValidentry = true;

            if (isValidentry) {
                $.each(result, function (key, value) {
                    ////if ($('input[name="ListDetails[0].ItemId"]').val() == 0) {

                    $("span." + key).find("span").text(value);
                    $("span." + key).find("input").val(value);
                    $("span." + key).find("textarea").val(value);
                    if ($("#" + key).val() == '') {
                        $("#" + key).val(value);
                    }

                    ////if (key == "ItemDescription" && value !== 'undefined') {
                    ////    $("span." + key).find("span").text($('input#ItemDescription').val());
                    ////}

                    ////if (key == "BusinessUnits" && value !== 'undefined') {
                    ////    $("span." + key).find("span").text($('input#BusinessUnits').val());
                    ////}

                    ////}
                });
            }
            else {
                HideWaitDialog();
                $('#ItemCode').val('');
                var date = '';
                if (result.LockingDate) {
                    var dateString = result.LockingDate.substr(6);
                    var currentTime = new Date(parseInt(dateString));
                    var month = currentTime.getMonth() + 1;
                    var day = currentTime.getDate();
                    var year = currentTime.getFullYear();
                    date = day + "/" + month + "/" + year;
                }
                var errMessage = "Selected item code is locked by Admin till " + date;
                AlertModal('Validation', errMessage);
            }

        }

        //BindApprover();

    });
    $("#SuggestedBy").blur();
}
function ItemCodeRemoved(ele) {

    $("#ItemCode").tokenInput("clear");
    $("#ItemCode").val("");

    $(".ItemCode").find('span').text('');
    $(".ItemCode").find('input').val('');
    $(".ItemCode").find("textarea").val('');

    //change done by priya starts here...
    //$(".ItemDescription").find('span').html('');
    //$(".BusinessUnits").find('span').html('');
    //changes done by priya end here....
}
function GetMultiselectValue(options) {
    var selected = '';
    options.each(function () {
        var label = ($(this).attr('label') !== undefined) ? $(this).attr('label') : $(this).text();
        selected += label + ",";
    });
    return selected.substr(0, selected.length - 1);
}

////Use this method in Feedback WIP Report
function ViewWIPReport(url) {
    url = SPHOSTURL + url;
    parent.postMessage(url, SPHOST);
    // window.location.href = url;
}
$(window).load(function () {
    // executes when complete page is fully loaded, including all frames, objects and images
    if ($("#SendBackToCC").is(":checked")) {
        //$("#closediv").css("visibility", "hidden");
        $("#closediv").hide();
        $("#quastatusdiv").css("visibility", "visible");
        $("a.btn:contains('Complete')").text("Send Back");
        $("a.btn:contains('Send Back')").attr('data-original-title', "Request will be forward to CC.");
        $("a.btn:contains('Send Back')").attr('data-action', "22");
    } else {
        $("#closediv").css("visibility", "visible");
        $("#quastatusdiv").css("visibility", "hidden");
        $("a.btn:contains('Send Back')").text("Complete");
        $("a.btn:contains('Complete')").attr('data-original-title', "Request will be close.");
        $("a.btn:contains('Complete')").attr('data-action', "14");
    }
   
});