const functions = require('firebase-functions');
const admin = require('firebase-admin');
admin.initializeApp(functions.config().firebase);

exports.TestCloudFunction = functions.database.ref('/Test/{testId}')
    .onWrite(event => {

    var original = event.data.current.child('a').val();

    var parentName = event.data.current.key;

    var multiplier = parseFloat(event.data.current.child('multiplier').val());

    var newValue = parseFloat(original)*multiplier;

    event.data.ref.child('parentName').set(parentName);

    return event.data.ref.child('b').set(newValue);

});



exports.AssignTeam = functions.database.ref('/Cricket/Tournament/{tournamentID}/{matchID}')
  .onCreate(event =>{
  var Team1 = event.data.current.child('Team1').val();
  var Team2 = event.data.current.child('Team2').val();


  admin.database().ref('Cricket/Teams/'+Team1).once('value')
  .then(function(snap){
      //event.data.ref.child('Team1').child(Team1).set(snap.val());
      event.data.ref.child('Team1Players').set(snap.val());
    });

  admin.database().ref('Cricket/Teams/'+Team2).once('value')
    .then(function(snap){
      //event.data.ref.child('Team2').child(Team2).set(snap.val());
      event.data.ref.child('Team2Players').set(snap.val());
      });
	  
  admin.database().ref('Cricket/FreeLeagues').once('value')
    .then(function(snap){
      //event.data.ref.child('Team2').child(Team2).set(snap.val());
      event.data.ref.child('FreeLeagues').set(snap.val());
      });
	  
  admin.database().ref('Cricket/PaidLeagues').once('value')
    .then(function(snap){
      //event.data.ref.child('Team2').child(Team2).set(snap.val());
      event.data.ref.child('PaidLeagues').set(snap.val());
      });
	  
});


