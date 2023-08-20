# Maui.Ideas.App
Maui application for personal feed explorer

## CollectionView Performance
Since we have issues with [Bindings](https://github.com/xamarin/Xamarin.Forms/issues/8718) and some [GC pressure on lists](https://codetraveler.io/2020/07/12/improving-collectionview-scrolling/)
I followed a "compile-time way", using the [Compiled Bindings](https://github.com/levitali/CompiledBindings/blob/ff0312acaebb0ee50665b51944cdcb7014d93eb7/README.md#performance-in-xamarin-forms-app) library to [reduce objects allocation](https://github.com/levitali/CompiledBindings/issues/4)

## LazyImageLoader
I've made a control, Proof of Concept, that downloads images and saves them locally for cache and shows an activity indicator when the download is in progress.
Isn't the best approach for this, since Glide already has a cache and some libraries (Like FFImageLoading) already can do cache and placeholder images.
But as a discovery alternative, without another assembly import, is a thing that can be studied 

## API Rest
I'm using the Refit + Polly combination to reduce code time and keep the best practices for network/rest and resilience approach.

![ezgif com-optimize](https://github.com/felipebaltazar/Maui.Ideas.App/assets/19656249/ff0e2658-56f2-486c-bcef-0977816aca52)
