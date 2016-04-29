module.exports = function(grunt) {

  // Project configuration.
  grunt.initConfig({
    pkg: grunt.file.readJSON('package.json'),
    less: {
        development: {
             options: {
                compress: false,
                ieCompat: true
             },
             files: {"public/css/style.css": "public/css/style.less"}
         },
         production: {
             options: {
                compress: true,
                ieCompat: true,
                cleancss: true
             },
             files: {"public/css/style.css": "public/css/style.less"}
         }

/*
         ,
         production: {
             options: {
                 paths: ["css"],
                 cleancss: true
             },
             files: {"style.css": "styleless"}
         }
         */
    },

    watch: {
      files: 'public/css/*.less',
      tasks: ['less:development'],
      options: {
          livereload: 1337 // set custom port to avoid conflict with other watch
      }
    },

    // watch our node server for changes
    nodemon: {
      development: {
        script: 'server.js'
      },
      production: {
        script: 'server.js'
      }
    },

    // set node environment
    env : {
      options : {
        //Shared Options Hash
      },
      development : {
        NODE_ENV : 'development'
      }
    },

    // run watch and nodemon at the same time
    concurrent: {
      development: {
        options: {
          logConcurrentOutput: true
        },
        tasks: ['nodemon:development', 'watch']
      },
      production: {
        options: {
          logConcurrentOutput: true
        },
        tasks: ['nodemon:production']
      }
    }

   });
  // Load the plugin that provides the "uglify" task.
  grunt.loadNpmTasks('grunt-contrib-less');
  grunt.loadNpmTasks('grunt-contrib-watch');
  grunt.loadNpmTasks('grunt-nodemon');
  grunt.loadNpmTasks('grunt-concurrent');
  grunt.loadNpmTasks('grunt-env');

  // Default task(s).
  grunt.registerTask('development', ['less:development', 'env:development', 'concurrent:development']);
  grunt.registerTask('production', ['less:production', 'concurrent:production']);

};

