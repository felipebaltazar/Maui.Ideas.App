# Maui.Ideas.App
[![Build and Unit Tests](https://github.com/felipebaltazar/Maui.Ideas.App/actions/workflows/dotnet.yml/badge.svg)](https://github.com/felipebaltazar/Maui.Ideas.App/actions/workflows/dotnet.yml)
<div align="center">

![ezgif com-optimize](https://github.com/felipebaltazar/Maui.Ideas.App/assets/19656249/ff0e2658-56f2-486c-bcef-0977816aca52)
 
</div>


Maui application for personal feed explorer using some coding and layout best practices.
Like async/await with ConfigureAwait for correct contexts. About this, i recommend this [great post from John Thiriet](https://johnthiriet.com/configure-await/) that explains context syncronization's impact on Xamarin.
[On NDC Olso, 2019 Brandom Minninck has talked about common mistakes](https://codetraveler.io/ndcoslo-asyncawait/) with async/await on .net, and before that he made a library to solve these situations

Also I've used the best practices for Xamarin/Maui layouts like:
 - [Choosing a correct layout for each situation](https://learn.microsoft.com/en-us/xamarin/xamarin-forms/deploy-test/performance#choose-the-correct-layout)
 - [Optimized the all layouts preventing to use AUTO (grid) and other heavy properties as possible ](https://learn.microsoft.com/en-us/xamarin/xamarin-forms/deploy-test/performance#optimize-layout-performance)
 - [I've used the Layout Compression as possible to reduce nested view hierarchy](https://learn.microsoft.com/en-us/xamarin/xamarin-forms/user-interface/layouts/layout-compression)

## CollectionView Performance
Since we have issues with [Bindings](https://github.com/xamarin/Xamarin.Forms/issues/8718) and some [GC pressure on lists](https://codetraveler.io/2020/07/12/improving-collectionview-scrolling/)
I followed a "compile-time way", using the [Compiled Bindings](https://github.com/levitali/CompiledBindings/blob/ff0312acaebb0ee50665b51944cdcb7014d93eb7/README.md#performance-in-xamarin-forms-app) library to [reduce objects allocation](https://github.com/levitali/CompiledBindings/issues/4)

## LazyImageLoader
I've made a control, Proof of Concept, that downloads images and saves them locally for cache and shows an activity indicator when the download is in progress.
Isn't the best approach for this, since Glide already has a cache and some libraries (Like FFImageLoading) that already can do cache and placeholder images.
But as a discovery alternative, without another assembly import, is a thing that can be studied 

## API Rest
I'm using the Refit + Polly combination to reduce code time and keep the best practices for network/rest and resilience approach.

## Unit Tests
I've made 1 unit test for MainPageViewModel using the technologies below:
- Fluent assertions
- MOQ

![image](https://github.com/felipebaltazar/Maui.Ideas.App/assets/19656249/ee2630a0-21eb-41a5-8dc0-c428a2417eac)

## CI/CD
Ive made a [simple workflow](https://github.com/felipebaltazar/Maui.Ideas.App/actions/runs/5920012382) to build maui android app and run unit tests



## UI Tests
I've made a [Proof of Concept using Maui + Xappim](https://mauicoder.com/xappium-for-android-testing-part-1-setup/),
and I already worked with Xamarin.UITest but [Xappium](https://xappium.com/) seems to be the future tool for the MAUI and its open source

