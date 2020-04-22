// products
var prodsApp = new Vue({
    el: '#prodsApp',
    data: {
        products: [],
        catId: ''
    },
    mounted() {
        this.catId = this.$refs.cat.attributes["id"].value;
        axios.get('/api/products/' + this.catId)
            .then(function (r) {
                if (r && r.data) {
                    r.data.forEach(p => {
                        prodsApp.products.push(p);
                        p.url = '/products/details/' + p.id;
                    });
                }
            })
            .catch(function (error) {
                console.log(error);
            });
    }
});

// product detail
var prodApp = new Vue({
    el: '#prodApp',
    data: {
        name: '',
        description: '',
        price: '',
        currency: ''
    },
    methods: {
        addToCart: function () {
            alert('todo');
        }
    },
    mounted() {
        axios.get('/api/products/details/asdfdfg')
            .then(function (r) {
                if (r && r.data) {
                    prodApp.name = r.data.name;
                    prodApp.description = r.data.description;
                    prodApp.price = r.data.price;
                    prodApp.currency = r.data.currency;
                }
            })
            .catch(function (error) {
                console.log(error);
            });
    }
});