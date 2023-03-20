//const form = document.querySelector('form');

//form.addEventListener('submit', e => {
//    e.preventDefault();

//    const values = Array.from(document.querySelectorAll('input[type=checkbox]:checked'))
//        .map(item => item.value)
//        .join(',');

//    console.log(values);
//});



//document.getElementById("myButton").addEventListener("click", function () {
//    var checkboxValue = document.getElementById("myCheckbox").checked;
//    alert("Checkbox value is: " + checkboxValue);
//});


function asdfg(e) {
    var a = e.checkboxValue();
    alert(a);
   

}


var itemForm = document.getElementById('itemForm'); // getting the parent container of all the checkbox inputs
var checkBoxes = itemForm.querySelectorAll('input[type="checkbox"]'); // get all the check box
document.getElementById('submit').addEventListener('click', getData); //add a click event to the save button

let result = [];

function getData() { // this function will get called when the save button is clicked
    result = [];
    checkBoxes.forEach(item => { // loop all the checkbox item
        if (item.checked) {  //if the check box is checked
            let data = {    // create an object
                item: item.value,
                selected: item.checked
            }
            result.push(data); //stored the objects to result array
        }
    })
    document.querySelector('.result').textContent = JSON.stringify(result); // display result
}





    var itemForm = document.getElementById('itemForm'); // getting the parent container of all the checkbox inputs
    var checkBoxes = itemForm.querySelectorAll('input[type="checkbox"]'); // get all the check box
    document.getElementById('submit').addEventListener('click', getData); //add a click event to the save button

    let results = [];

function getData() { // this function will get called when the save button is clicked
    result = [];
    checkBoxes.forEach(item => { // loop all the checkbox item
        if (item.checked) {  //if the check box is checked
            let data = {    // create an object
                item: item.value,
                selected: item.checked
            }
            result.push(data); //stored the objects to result array
        }
    })
    document.querySelector('.results').textContent = JSON.stringify(results); // display result
}




