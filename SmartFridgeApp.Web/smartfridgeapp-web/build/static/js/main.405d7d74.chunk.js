(this.webpackJsonpapp=this.webpackJsonpapp||[]).push([[0],{135:function(e,t,c){},136:function(e,t,c){},164:function(e,t,c){"use strict";c.r(t);var n=c(3),a=c(1),r=c.n(a),o=c(11),i=c.n(o),s=(c(132),c(133),c(134),c(135),c(20)),d=c(17);c(136);var l=function(){return Object(n.jsxs)("div",{className:"About",children:[Object(n.jsx)("h1",{children:"About page"}),Object(n.jsx)(s.b,{to:"/",children:"Go to Home page"})]})},j=c(30),u=c(23),b=c(9),O=c(18),f=c(24),h=c(112),m=c.n(h),x=c(34),p=c.n(x),g=c(204),v=c(214),y=c(219),C=c(222),S=c(213),I=c(211),N=c(210),R=c(32),E=c.n(R),P=c(53),k=c.n(P),A=c(113),V=c.n(A),U=[{label:"#",field:"id",sort:"asc"},{label:"Name",field:"name",sort:"asc"},{label:"Address",field:"address",sort:"asc"},{label:"",field:"buttons",sort:"asc"},{label:"",field:"delete",sort:"asc"}],L=function(){var e=Object(a.useState)(!0),t=Object(b.a)(e,2),c=t[0],r=t[1],o=Object(a.useState)(1),i=Object(b.a)(o,2),d=i[0],l=i[1],h=Object(a.useState)(!1),x=Object(b.a)(h,2),R=x[0],P=x[1],A=Object(a.useState)({name:"",address:"",desc:""}),L=Object(b.a)(A,2),F=L[0],w=L[1],_=Object(a.useState)([{id:1,name:"fridge",address:"",buttons:Object(n.jsx)(s.b,{to:"/"}),delete:Object(n.jsx)(g.a,{})}]),T=Object(b.a)(_,2),D=T[0],J=T[1],q=function(e){var t=e.target,c=t.name,n=t.value;w(Object(u.a)(Object(u.a)({},F),{},Object(j.a)({},c,n)))},W=function(){B(),P(!1)},B=function(){w({name:"",address:"",desc:""})};return Object(a.useEffect)((function(){fetch(O.SERVER_URL+"/api/fridges").then((function(e){return e.json()})).then((function(e){var t=e.map((function(e,t){return{id:t+1,name:e.name,address:e.address,buttons:Object(n.jsx)(s.b,{to:"/fridgeitems/".concat(e.id),children:Object(n.jsx)(g.a,{variant:"contained",color:"primary",startIcon:Object(n.jsx)(m.a,{}),children:"Details"})}),delete:Object(n.jsx)(g.a,{variant:"contained",color:"secondary",startIcon:Object(n.jsx)(V.a,{}),onClick:function(){return function(e){var t={fridgeId:e};fetch(O.SERVER_URL+"/api/fridges",{method:"delete",headers:{"Content-Type":"application/json"},body:JSON.stringify(t)})}(e.id)},children:"Remove"})}}));J(t)})).catch((function(e){return console.error(e)})).finally((function(){return r(!1)}))}),[d]),Object(n.jsxs)("div",{children:[Object(n.jsxs)(C.a,{open:R,onClose:W,"aria-labelledby":"form-dialog-title",children:[Object(n.jsx)(N.a,{id:"form-dialog-title",children:"Add new Fridge"}),Object(n.jsxs)(I.a,{children:[Object(n.jsx)(y.a,{name:"name",label:"Fridge name",fullWidth:!0,onChange:q,value:F.name}),Object(n.jsx)(y.a,{name:"address",label:"Address",fullWidth:!0,onChange:q,value:F.address}),Object(n.jsx)(y.a,{name:"desc",label:"Description",fullWidth:!0,onChange:q,value:F.desc})]}),Object(n.jsxs)(S.a,{children:[Object(n.jsx)(g.a,{onClick:W,color:"secondary",variant:"outlined",startIcon:Object(n.jsx)(E.a,{}),children:"Cancel"}),Object(n.jsx)(g.a,{onClick:function(){var e={name:F.name,address:F.address,desc:F.desc};fetch(O.SERVER_URL+"/api/fridges",{method:"post",headers:{"Content-Type":"application/json"},body:JSON.stringify(e)}).then(B()).then(P(!1))},color:"primary",startIcon:Object(n.jsx)(p.a,{}),variant:"outlined",children:"Add"})]})]}),Object(n.jsx)("br",{}),Object(n.jsxs)("div",{className:"btn-group userBtns",children:[Object(n.jsx)(g.a,{variant:"outlined",color:"primary",onClick:function(){P(!0)},startIcon:Object(n.jsx)(p.a,{}),children:"Add new"}),Object(n.jsx)(g.a,{variant:"outlined",color:"primary",onClick:function(){l(d+1)},startIcon:Object(n.jsx)(k.a,{}),children:"Refresh"})]}),Object(n.jsx)("br",{}),Object(n.jsx)("br",{}),c?Object(n.jsxs)("div",{children:[Object(n.jsx)(v.a,{}),Object(n.jsx)("p",{children:"Loading fridges"})]}):Object(n.jsxs)(f.g,{btn:!0,children:[Object(n.jsx)(f.i,{columns:U}),Object(n.jsx)(f.h,{rows:D})]})]})},F=c(75),w=c(115),_=c.n(w),T=function(e){var t=e.fridgeId,c=e.state,r=e.handleClose,o=Object(a.useState)({name:"",email:""}),i=Object(b.a)(o,2),s=i[0],d=i[1],l=function(e){var t=e.target,c=t.name,n=t.value;d(Object(u.a)(Object(u.a)({},s),{},Object(j.a)({},c,n)))};return Object(n.jsxs)(C.a,{open:c,onClose:r,"aria-labelledby":"form-dialog-title",children:[Object(n.jsx)(N.a,{id:"form-dialog-title",children:"Add user"}),Object(n.jsxs)(I.a,{children:[Object(n.jsx)(y.a,{name:"name",label:"Name",fullWidth:!0,onChange:l,value:s.name}),Object(n.jsx)(y.a,{name:"email",label:"Email",fullWidth:!0,onChange:l,value:s.email})]}),Object(n.jsxs)(S.a,{children:[Object(n.jsx)(g.a,{onClick:r,color:"secondary",variant:"outlined",startIcon:Object(n.jsx)(E.a,{}),children:"Cancel"}),Object(n.jsx)(g.a,{color:"primary",startIcon:Object(n.jsx)(p.a,{}),variant:"outlined",onClick:function(){var e={user:{name:s.name,email:s.email}};fetch(O.SERVER_URL+"/api/fridgeUsers/"+t,{method:"post",headers:{"Content-Type":"application/json"},body:JSON.stringify(e)}).then(void d({name:"",email:""})).then((function(){return r()}))},children:"Add"})]})]})},D=c(220),J=c(215),q=c(223),W=c(56),B=function(e){var t=e.fridgeId,c=e.selectedUserId,r=e.state,o=e.handleClose,i=Object(a.useState)([{foodProductId:1,foodProductName:"fp"}]),s=Object(b.a)(i,2),d=s[0],l=s[1],f=Object(a.useState)(0),h=Object(b.a)(f,2),m=h[0],x=h[1],v=Object(a.useState)({note:"",value:""}),R=Object(b.a)(v,2),P=R[0],k=R[1],A=Object(a.useState)("NotAssigned"),V=Object(b.a)(A,2),U=V[0],L=V[1];Object(a.useEffect)((function(){console.log("food products fetch"),fetch(O.SERVER_URL+"/api/foodProducts").then((function(e){return e.json()})).then((function(e){var t=e.map((function(e,t){return{foodProductId:e.foodProductId,foodProductName:e.foodProductName}}));l(t)})).catch((function(e){return console.error(e)}))}),[]);return Object(n.jsxs)(C.a,{open:r,onClose:o,"aria-labelledby":"form-dialog-title",children:[Object(n.jsx)(N.a,{id:"form-dialog-title",children:"Add Fridge item"}),Object(n.jsxs)(I.a,{children:[Object(n.jsx)(D.a,{id:"foodproducts-combobox",options:d,getOptionLabel:function(e){return e.foodProductName},style:{width:"100%"},onChange:function(e,t){void 0!=t&&null!=t&&x(t.foodProductId)},renderInput:function(e){return Object(n.jsx)(y.a,Object(u.a)(Object(u.a)({},e),{},{label:"Select food product",variant:"outlined"}))}}),Object(n.jsx)("br",{}),Object(n.jsx)(y.a,{name:"value",label:"Value",fullWidth:!0,onChange:function(e){if(""===e.target.value||/^[0-9\b]+$/.test(e.target.value)){var t=e.target,c=t.name,n=t.value;k(Object(u.a)(Object(u.a)({},P),{},Object(j.a)({},c,n)))}},value:P.value}),Object(n.jsx)("br",{}),Object(n.jsx)("br",{}),Object(n.jsx)("br",{}),Object(n.jsxs)(q.a,{value:U,exclusive:!0,onChange:function(e,t){L(t)},"aria-label":"text alignment",children:[Object(n.jsx)(J.a,{value:"Grams","aria-label":"left aligned",children:Object(n.jsx)(W.a,{variant:"body1",children:"Grams"})}),Object(n.jsx)(J.a,{value:"Pieces","aria-label":"centered",children:Object(n.jsx)(W.a,{children:"Pieces"})}),Object(n.jsx)(J.a,{value:"Mililiter","aria-label":"right aligned",children:Object(n.jsx)(W.a,{children:"Mililiter"})}),Object(n.jsx)(J.a,{value:"NotAssigned","aria-label":"justified",children:Object(n.jsx)(W.a,{children:"None"})})]}),Object(n.jsx)("br",{}),Object(n.jsx)("br",{})]}),Object(n.jsxs)(S.a,{children:[Object(n.jsx)(g.a,{onClick:o,color:"secondary",variant:"outlined",startIcon:Object(n.jsx)(E.a,{}),children:"Cancel"}),Object(n.jsx)(g.a,{onClick:function(){var e={userId:c,fridgeItem:{foodProductId:m,value:parseInt(P.value),note:"",unit:U}};console.log(e),fetch(O.SERVER_URL+"/api/fridgeItems/"+t+"/add",{method:"post",headers:{"Content-Type":"application/json"},body:JSON.stringify(e)}).then((k({note:"",unit:"",value:""}),void x(0))).then(console.log("Success")).then((function(){return o()}))},color:"primary",startIcon:Object(n.jsx)(p.a,{}),variant:"outlined",children:"Add"})]})]})},G=[{label:"#",field:"id",sort:"asc"},{label:"Product",field:"productName",sort:"asc"},{label:"Category",field:"categoryName",sort:"asc"},{label:"Value",field:"value",sort:"asc"},{label:"Unit",field:"unit",sort:"asc"},{label:"#",field:"consume",sort:"asc"}],M=function(){var e=Object(d.g)().fridgeId,t=Object(a.useState)(1),c=Object(b.a)(t,2),r=c[0],o=c[1],i=Object(a.useState)(!1),s=Object(b.a)(i,2),l=s[0],j=s[1],u=Object(a.useState)(!1),h=Object(b.a)(u,2),m=h[0],x=h[1],p=Object(a.useState)(""),C=Object(b.a)(p,2),S=C[0],I=(C[1],Object(a.useState)(!0)),N=Object(b.a)(I,2),R=N[0],E=N[1],P=Object(a.useState)(!0),A=Object(b.a)(P,2),V=A[0],U=A[1],L=Object(a.useState)([]),w=Object(b.a)(L,2),D=w[0],J=w[1],q=Object(a.useState)("None"),W=Object(b.a)(q,2),M=W[0],z=W[1],$=Object(a.useState)([{id:1,categoryName:"category",productName:"product",value:10,unit:"unit",consume:Object(n.jsx)("div",{children:Object(n.jsx)(g.a,{gradient:"aqua",size:"sm",children:"Consume"})})}]),H=Object(b.a)($,2),K=H[0],Q=H[1];Object(a.useEffect)((function(){console.log("useEffect fridgeItem"),"None"!=M&&fetch(O.SERVER_URL+"/api/fridgeItems/"+e+"/"+M).then((function(e){return e.json()})).then((function(e){var t=e.map((function(e,t){return{id:t+1,productName:e.productName,categoryName:e.categoryName,value:e.value,unit:e.unit,consume:Object(n.jsxs)("span",{children:[Object(n.jsx)(y.a,{name:"amount",label:"Amount",fullWidth:!0,onChange:X,value:S}),Object(n.jsx)(g.a,{variant:"contained",color:"primary",startIcon:Object(n.jsx)(_.a,{}),onClick:function(){return Y(e.fridgeItemId,e.unit)},children:"Consume"})]})}}));Q(t)})).catch((function(e){return console.error(e)})).finally((function(){return U(!1)}))}),[M,r]),Object(a.useEffect)((function(){console.log("useEffect users"),fetch(O.SERVER_URL+"/api/fridgeUsers/"+e).then((function(e){return e.json()})).then((function(e){J([{id:"None",name:"None"}].concat(Object(F.a)(e)))})).catch((function(e){return console.error(e)})).finally((function(){return E(!1)}))}),[r]);var X=function(e){(""===e.target.value||/^[0-9\b]+$/.test(e.target.value))&&console.log(e.target.value)},Y=function(t,c){var n={fridgeItemId:t,userId:M,amountValue:{value:0,unit:c}};console.log(n),fetch(O.SERVER_URL+"/api/fridgeItems/"+e+"/consume",{method:"post",headers:{"Content-Type":"application/json"},body:JSON.stringify(n)}).then(console.log("Success Consume"))};return Object(n.jsxs)("div",{children:[Object(n.jsx)(T,{fridgeId:e,state:l,handleClose:function(){j(!1)}}),Object(n.jsx)(B,{fridgeId:e,selectedUserId:M,state:m,handleClose:function(){x(!1)}}),R?Object(n.jsxs)("div",{children:[Object(n.jsx)(v.a,{}),Object(n.jsx)("p",{children:"Loading users"})]}):Object(n.jsx)("div",{className:"btn-group userBtns",children:D.map((function(e){return Object(n.jsx)(g.a,{variant:"contained",color:M===e.id?"secondary":"primary",onClick:function(){return z(e.id)},children:e.name})}))}),Object(n.jsx)("div",{children:Object(n.jsxs)("div",{className:"btn-group userBtns",children:[Object(n.jsx)(g.a,{variant:"outlined",color:"primary",onClick:function(){return x(!0)},children:"Add new fridge item"}),Object(n.jsx)(g.a,{variant:"outlined",color:"primary",onClick:function(){return j(!0)},children:"Add new user"}),Object(n.jsx)(g.a,{variant:"outlined",color:"primary",onClick:function(){o(r+1)},startIcon:Object(n.jsx)(k.a,{}),children:"Refresh"})]})}),Object(n.jsx)("br",{}),V?Object(n.jsx)("p",{children:"Select user"}):Object(n.jsxs)(f.g,{children:[Object(n.jsx)(f.i,{columns:G}),Object(n.jsx)(f.h,{rows:K})]})]})},z=c(89),$=(c(163),function(e){var t=e.categories,c=e.state,o=e.handleClose,i=Object(a.useState)({name:""}),s=Object(b.a)(i,2),d=s[0],l=s[1],f=Object(a.useState)(0),h=Object(b.a)(f,2),m=h[0],x=h[1],v=function(e){return Object(z.b)(e)};return Object(n.jsxs)(r.a.Fragment,{children:[Object(n.jsx)(z.a,{}),Object(n.jsxs)(C.a,{open:c,onClose:o,"aria-labelledby":"form-dialog-title",children:[Object(n.jsx)(N.a,{id:"form-dialog-title",children:"Add food product"}),Object(n.jsxs)(I.a,{children:[Object(n.jsx)(y.a,{name:"name",label:"Name",fullWidth:!0,onChange:function(e){var t=e.target,c=t.name,n=t.value;l(Object(u.a)(Object(u.a)({},d),{},Object(j.a)({},c,n)))},value:d.name}),Object(n.jsx)("br",{}),Object(n.jsx)("br",{}),Object(n.jsx)(D.a,{id:"categories-combobox",options:t,getOptionLabel:function(e){return e.name},style:{width:"100%"},onChange:function(e,t){void 0!=t&&null!=t&&x(t.foodProductId)},renderInput:function(e){return Object(n.jsx)(y.a,Object(u.a)(Object(u.a)({},e),{},{label:"Select category",variant:"outlined"}))}})]}),Object(n.jsxs)(S.a,{children:[Object(n.jsx)(g.a,{onClick:o,color:"secondary",variant:"outlined",startIcon:Object(n.jsx)(E.a,{}),children:"Cancel"}),Object(n.jsx)(g.a,{color:"primary",startIcon:Object(n.jsx)(p.a,{}),variant:"outlined",onClick:function(){var e={name:d.name,category:m};fetch(O.SERVER_URL+"/api/foodProducts",{method:"post",headers:{"Content-Type":"application/json"},body:JSON.stringify(e)}).then((function(e){if(!e.ok)throw v("Cant add food product."),Error(e.statusText);return v("Food product added."),e})).then(void l({name:"",category:""})).catch((function(e){console.log(e)})).then((function(){return o()}))},children:"Add"})]})]})]})}),H=[{label:"#",field:"id",sort:"asc"},{label:"Name",field:"name",sort:"asc"},{label:"Category",field:"category",sort:"asc"}],K=function(){var e=Object(a.useState)(!0),t=Object(b.a)(e,2),c=t[0],r=t[1],o=Object(a.useState)(!1),i=Object(b.a)(o,2),s=i[0],d=i[1],l=Object(a.useState)({name:"categoryName",categoryId:1}),j=Object(b.a)(l,2),u=j[0],h=j[1],m=Object(a.useState)(1),x=Object(b.a)(m,2),p=x[0],y=x[1],C=Object(a.useState)([{id:1,name:"foodProducts",category:"category"}]),S=Object(b.a)(C,2),I=S[0],N=S[1];Object(a.useEffect)((function(){fetch(O.SERVER_URL+"/api/foodproducts").then((function(e){return e.json()})).then((function(e){var t=e.map((function(e,t){return{id:t+1,name:e.foodProductName,category:e.foodProductCategory}}));N(t)})).catch((function(e){return console.error(e)})).finally((function(){return r(!1)})),fetch(O.SERVER_URL+"/api/foodproducts/categories").then((function(e){return e.json()})).then((function(e){var t=e.map((function(e){return{name:e.name,categoryId:e.categoryId}}));h(t)})).catch((function(e){return console.error(e)}))}),[p]);return Object(n.jsxs)("div",{children:[Object(n.jsx)($,{categories:u,state:s,handleClose:function(){d(!1)}}),Object(n.jsx)(g.a,{variant:"outlined",color:"primary",onClick:function(){return d(!0)},children:"Add new food product"}),Object(n.jsx)(g.a,{variant:"outlined",color:"primary",onClick:function(){y(p+1)},startIcon:Object(n.jsx)(k.a,{}),children:"Refresh"}),c?Object(n.jsxs)("div",{children:[Object(n.jsx)(v.a,{}),Object(n.jsx)("p",{children:"Loading food products"})]}):Object(n.jsxs)(f.g,{btn:!0,children:[Object(n.jsx)(f.i,{columns:H}),Object(n.jsx)(f.h,{rows:I})]})]})},Q=c(116),X=c.n(Q),Y=c(212),Z=c(216),ee=c(217),te=c(218),ce=[{label:"#",field:"id",sort:"asc"},{label:"Name",field:"name",sort:"asc"},{label:"Category",field:"category",sort:"asc"},{label:"Required time",field:"reqTime",sort:"asc"},{label:"Difficulty",field:"difficulty",sort:"asc"},{label:"#",field:"details",sort:"asc"}],ne=function(){var e=Object(a.useState)(!0),t=Object(b.a)(e,2),c=t[0],r=t[1],o=Object(a.useState)(!1),i=Object(b.a)(o,2),s=i[0],d=i[1],l=Object(a.useState)([{id:1,name:"name",category:"cat",reqTime:"req",difficulty:"diff",details:Object(n.jsx)(g.a,{gradient:"aqua",size:"sm",children:"Details"})}]),j=Object(b.a)(l,2),u=j[0],h=j[1],m=Object(a.useState)({recipeName:"recipe",recipeDescription:"desc",foodProducts:"items"}),x=Object(b.a)(m,2),p=x[0],y=x[1],R=Object(a.useState)([{foodProductId:"1",foodProductName:"fpName1",AmountValue:{Value:"1",Unit:"Grams"}}]),P=Object(b.a)(R,2),k=P[0],A=P[1];var V=function(e,t,c){y({recipeName:e,recipeDescription:t,foodProducts:c}),function(e){var t=e,c=JSON.parse(t).ArrayOfFoodProductDetails.FoodProductDetails;A([]);for(var n=function(){var e=Object(b.a)(r[a],2),t=(e[0],e[1]);A((function(e){return[].concat(Object(F.a)(e),[{FoodProductId:t.FoodProductId,FoodProductName:t.FoodProductName,AmountValue:{Value:t.AmountValue.Value,Unit:t.AmountValue.Unit}}])}))},a=0,r=Object.entries(c);a<r.length;a++)n()}(c),d(!0)},U=function(){d(!1)};return Object(a.useEffect)((function(){fetch(O.SERVER_URL+"/api/recipes").then((function(e){return e.json()})).then((function(e){var t=e.map((function(e,t){return{id:t+1,name:e.recipeName,category:e.recipeCategory,reqTime:e.requiredTime,difficulty:e.levelOfDifficulty,details:Object(n.jsx)(g.a,{variant:"contained",color:"primary",startIcon:Object(n.jsx)(X.a,{}),onClick:function(){return V(e.recipeName,e.description,e.foodProducts)},children:"Details"})}}));h(t)})).catch((function(e){return console.error(e)})).finally((function(){return r(!1)}))}),[]),Object(n.jsxs)("div",{children:[Object(n.jsxs)(C.a,{open:s,onClose:U,"aria-labelledby":"form-dialog-title",children:[Object(n.jsx)(N.a,{id:"form-dialog-title",children:p.recipeName}),Object(n.jsxs)(I.a,{dividers:!0,children:[Object(n.jsx)(Y.a,{component:"nav","aria-label":"secondary mailbox folders",children:k.map((function(e){return Object(n.jsx)(Z.a,{children:Object(n.jsxs)(ee.a,{children:["- ",e.foodProductName," ",e.AmountValue.Value," ",e.AmountValue.Unit]})},e.FoodProductId)}))}),Object(n.jsx)("br",{}),Object(n.jsx)(te.a,{children:p.recipeDescription})]}),Object(n.jsxs)(S.a,{children:[Object(n.jsx)(g.a,{onClick:U,color:"secondary",variant:"outlined",startIcon:Object(n.jsx)(E.a,{}),children:"Cancel"}),Object(n.jsx)(g.a,{onClick:function(){console.log("use this recipe")},color:"primary",variant:"outlined",children:"Use this recipe"})]})]}),c?Object(n.jsxs)("div",{children:[Object(n.jsx)(v.a,{}),Object(n.jsx)("p",{children:"Loading recipes"})]}):Object(n.jsxs)(f.g,{btn:!0,children:[Object(n.jsx)(f.i,{columns:ce}),Object(n.jsx)(f.h,{rows:u})]})]})};var ae=function(){return Object(n.jsxs)("nav",{children:[Object(n.jsx)(s.c,{activeClassName:"active",to:"/fridges",className:"header-links",children:"Fridges"}),Object(n.jsx)(s.c,{activeClassName:"active",to:"/foodProducts",className:"header-links",children:"FoodProducts"}),Object(n.jsx)(s.c,{activeClassName:"active",to:"/recipes",className:"header-links",children:"Recipes"})]})};var re=function(){return Object(n.jsx)("div",{className:"App",children:Object(n.jsx)(s.a,{children:Object(n.jsxs)("div",{children:[Object(n.jsx)(ae,{}),Object(n.jsxs)(d.d,{children:[Object(n.jsx)(d.b,{exact:!0,path:"/",children:Object(n.jsx)(d.a,{to:"/fridges"})}),Object(n.jsx)(d.b,{path:"/recipes",children:Object(n.jsx)(ne,{})}),Object(n.jsx)(d.b,{path:"/fridges",children:Object(n.jsx)(L,{})}),Object(n.jsx)(d.b,{path:"/foodProducts",children:Object(n.jsx)(K,{})}),Object(n.jsx)(d.b,{path:"/fridgeitems/:fridgeId",children:Object(n.jsx)(M,{})}),Object(n.jsx)(d.b,{path:"/about",children:Object(n.jsx)(l,{})})]})]})})})},oe=function(e){e&&e instanceof Function&&c.e(3).then(c.bind(null,225)).then((function(t){var c=t.getCLS,n=t.getFID,a=t.getFCP,r=t.getLCP,o=t.getTTFB;c(e),n(e),a(e),r(e),o(e)}))};i.a.render(Object(n.jsx)(r.a.StrictMode,{children:Object(n.jsx)(re,{})}),document.getElementById("root")),oe()},18:function(e){e.exports=JSON.parse('{"SERVER_URL":"https://smartfridgeapp.pl"}')}},[[164,1,2]]]);
//# sourceMappingURL=main.405d7d74.chunk.js.map