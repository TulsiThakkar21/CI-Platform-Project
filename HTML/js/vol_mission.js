let items = document.querySelectorAll('.carousel .carousel-item')

items.forEach((el) => {
    const minPerSlide = 4
    let next = el.nextElementSibling
    for (var i=1; i<minPerSlide; i++) {
        if (!next) {
            // wrap carousel by using first child
        	next = items[0]
      	}
        let cloneChild = next.cloneNode(true)
        el.appendChild(cloneChild.children[0])
        next = next.nextElementSibling
    }
})





// To change the img on click

var photo = document.getElementById("changingimg");
function funz1() {

photo.src = "/images/vol-carousel-1.png" ;

}


function funz2() {

photo.src = "/images/vol-carrousel-2.png" ;

}

function funz3() {

photo.src = "/images/volcarousel-3.png" ;

}


function funz4() {

photo.src = "/images/vol-carousel-4.png" ;

}