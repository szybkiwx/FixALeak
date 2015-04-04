var gulp = require('gulp');
var bower = require('gulp-bower');
var jshint = require("gulp-jshint");
var sass = require('gulp-sass');
var concat = require('gulp-concat');
var uglify = require('gulp-uglify');
var rename = require('gulp-rename');

gulp.task('bower', function () {
    return bower();
});

var paths = {
    scripts: 'app/**/*.js'
};

gulp.task('bower', function () {
    return bower()
      .pipe(gulp.dest('dist/lib/'))
});

gulp.task('lint', function () {
    return gulp.src(paths.scripts)
        .pipe(jshint())
        .pipe(jshint.reporter('default'));
});

gulp.task('sass', function () {
    return gulp.src('app/scss/*.scss')
        .pipe(sass())
        .pipe(gulp.dest('dist/css'));
});

gulp.task('scripts', function () {
    return gulp.src('app/**/*.js')
        .pipe(concat('all.js'))
        .pipe(gulp.dest('dist/js'))
});

gulp.task('minify', function () {
    return gulp.src('dist/js/all.js')
        .pipe(concat('all.js'))
        .pipe(gulp.dest('dist/js'))
        .pipe(uglify())
});


// Watch Files For Changes
gulp.task('watch', function () {
    gulp.watch('app/**/*.js', ['lint', 'scripts']);
    gulp.watch('app/scss/*.scss', ['sass']);
});


gulp.task('package', ['default', 'lint', 'sass', 'scripts', 'minify']);
// Default Task
gulp.task('default', ['bower', 'lint', 'sass', 'scripts']);

