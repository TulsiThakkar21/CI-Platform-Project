function submitstate() {
    document.getElementById('submitbtns').disabled = true;
    document.getElementById('submitbtns').style.borderColor = "grey";
    document.getElementById('submitbtns').style.color = "grey";
}

function addstory() {
    var abcd = $('#exampleFormControlSelect12').val();
    var videourl = document.getElementById('vidUrl').value;
    var storyTitle = document.getElementById('sTitle').value;
    var pubDate = document.getElementById('sPDate').value;
    var desc = document.getElementById('sDesc').value;
    const selectElement = document.getElementById("exampleFormControlSelect12");
    const missiondd = selectElement.selectedOptions[0].getAttribute("id");
    var fileInput = document.getElementById('fileElem');

    var files = fileInput.files;
    var formData = new FormData();

    for (var i = 0; i < files.length; i++) {

   
        formData.append('files', files[i]);
        console.log(formData.values);
    }
    formData.append('videourl', videourl);
    formData.append('abcd', abcd);
    formData.append('storyTitle', storyTitle);
    formData.append('pubDate', pubDate);
    formData.append('desc', desc);
    formData.append('selectElement', selectElement);
    formData.append('missiondd', missiondd);


    var xhr = new XMLHttpRequest();
    xhr.open('POST', '/Home/ShareYourStory2', true);
    xhr.onload = function () {
        if (xhr.status === 200) {
            // Handle success
            console.log('Files uploaded successfully.');
        } else {
            // Handle error
            console.error('Error uploading files.');
        }

    };
    xhr.send(formData);





    if (missiondd != null) {
        document.getElementById('submitbtns').disabled = false;
        document.getElementById('submitbtns').style.borderColor = "orange";
        document.getElementById('submitbtns').style.color = "orange";

    }
    $.ajax({
        url: '/Home/ShareStory2',
        type: "POST",
        data: {
            missiondd: missiondd,
            storyTitle: storyTitle,
            pubDate: pubDate,
            desc: desc,
            abcd: abcd,
            videourl: videourl,
            newArray: newArray,
            formData: formData

        },
        success: function (response) {
            console.log(1);
            /* document.getElementById("sTitle").innerHTML = response.sTitle;*/
        },
        error: function (xhr, textStatus, errorThrown) {
            console.log("Error: " + errorThrown);
        }
    });


}


let dropArea = document.getElementById("drop-area")

    // Prevent default drag behaviors
    ;['dragenter', 'dragover', 'dragleave', 'drop'].forEach(eventName => {
        dropArea.addEventListener(eventName, preventDefaults, false)
        document.body.addEventListener(eventName, preventDefaults, false)
    })

    // Highlight drop area when item is dragged over it
    ;['dragenter', 'dragover'].forEach(eventName => {
        dropArea.addEventListener(eventName, highlight, false)
    })

    ;['dragleave', 'drop'].forEach(eventName => {
        dropArea.addEventListener(eventName, unhighlight, false)
    })

// Handle dropped files
dropArea.addEventListener('drop', handleDrop, false)

function preventDefaults(e) {
    e.preventDefault()
    e.stopPropagation()
}

function highlight(e) {
    dropArea.classList.add('highlight')
}

function unhighlight(e) {
    dropArea.classList.remove('active')
}

function handleDrop(e) {
    var dt = e.dataTransfer
    var files = dt.files

    handleFiles(files)
}

let uploadProgress = []
let progressBar = document.getElementById('progress-bar')

function initializeProgress(numFiles) {
    progressBar.value = 0
    uploadProgress = []

    for (let i = numFiles; i > 0; i--) {
        uploadProgress.push(0)
    }
}

function updateProgress(fileNumber, percent) {
    uploadProgress[fileNumber] = percent
    let total = uploadProgress.reduce((tot, curr) => tot + curr, 0) / uploadProgress.length
    progressBar.value = total
}



function previewFile(file) {
    let reader = new FileReader()
    reader.readAsDataURL(file)
    reader.onloadend = function () {
        let img = document.createElement('img')
        img.src = reader.result
        document.getElementById('gallery').appendChild(img)
    }
}

function uploadFile(file, i) {
    var url = 'https://api.cloudinary.com/v1_1/joezimim007/image/upload'
    var xhr = new XMLHttpRequest()
    var formData = new FormData()
    xhr.open('POST', url, true)
    xhr.setRequestHeader('X-Requested-With', 'XMLHttpRequest')

    // Update progress (can be used to show progress indicator)
    xhr.upload.addEventListener("progress", function (e) {
        updateProgress(i, (e.loaded * 100.0 / e.total) || 100)
    })

    xhr.addEventListener('readystatechange', function (e) {
        if (xhr.readyState == 4 && xhr.status == 200) {
            updateProgress(i, 100) // <- Add this
        }
        else if (xhr.readyState == 4 && xhr.status != 200) {
            // Error. Inform the user
        }
    })

    formData.append('upload_preset', 'ujpu6gyk')
    formData.append('file', file)
    xhr.send(formData)
}


function canceldata() {
    /* $('#exampleFormControlSelect12').val() = "";*/
    document.getElementById('exampleFormControlSelect12').value = "";
    document.getElementById('sTitle').value = "";
    document.getElementById('sPDate').value = "";
    document.getElementById('sDesc').value = "";

    document.getElementById('vidUrl').value = "";
}


function newselecId(s) {


    const selectElement = document.getElementById("exampleFormControlSelect12");
    const selectedOptionId = selectElement.selectedOptions[0].getAttribute("id");
    console.log(selectedOptionId);

    $.ajax({
        url: '/Home/EditStory',
        type: "POST",
        data: {

            selectedOptionId: selectedOptionId

        },
        success: function (data) {
            console.log(data);
            //var a = new Date(data.publishedAt).toISOString().slice(0, 10);

            document.getElementById("sTitle").value = data.obj1.title;
            //document.getElementById("sPDate").value = data.publishedAt;
            document.getElementById("sDesc").innerHTML = data.obj1.description;
            document.getElementById("vidUrl").value = data.obj1.vidUrl;


            var filePath = data.obj2.storyPath;
            console.log(data.storypath);
            for (var i = 0; i < data.obj2.length; i++) {
                var img = document.createElement('img');
                img.src = data.obj2[i];
                img.id = i;
                /* img.onclick = removeimg;*/


                var photoContainer = document.getElementById('drop-area');
                photoContainer.appendChild(img);

            }


        },
        error: function (xhr, textStatus, errorThrown) {

            console.log("Error: " + errorThrown + textStatus + xhr);
        }
    });

}