$(document).ready(function () {

    if ($("#BulkUpload").length > 0) {
        BindFileUploadControl({
            ElementId: 'BulkUpload', Params: {}, Url: "UploadSKUMasterData",
            AllowedExtensions: ["csv","xls","xlsx"],
            ButtonText: "UPLOAD File",
            MultipleFiles: false,
            DuplicateCheck: false,
            CallBack: "onUploadedBulkData"
        });
    }
});

function onUploadedBulkData(data) {    
    if (data.Status==true) {
        AlertModal('Success', data.Message);
        $("#returnmsg").html(data.strHTML);
        //Exit();
    } else {
        AlertModal('Error', data.Message);
        
    }

}
