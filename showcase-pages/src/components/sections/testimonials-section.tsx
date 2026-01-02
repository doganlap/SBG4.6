'use client'

import { Badge } from '@/components/ui/badge'
import { Card, CardContent } from '@/components/ui/card'
import { Star } from 'lucide-react'

const testimonials = [
  {
    quote: "ERPNext SaaS transformed our operations. We went from 5 different tools to one integrated platform. The ROI was visible within the first month.",
    author: "Sarah Chen",
    role: "CEO",
    company: "TechStart Inc",
    rating: 5,
  },
  {
    quote: "The Healthcare module is incredibly comprehensive. Patient management, appointments, lab tests - everything our clinic needs in one place.",
    author: "Dr. Michael Roberts",
    role: "Medical Director",
    company: "MediCare Hospital",
    rating: 5,
  },
  {
    quote: "As a manufacturing company, the Production Planning and BOM features are game-changers. We've reduced waste by 30% since implementing.",
    author: "James Wilson",
    role: "Operations Manager",
    company: "Global Industries",
    rating: 5,
  },
  {
    quote: "The Education module handles everything from student enrollment to fee management. Our administrative workload dropped by 60%.",
    author: "Dr. Emily Johnson",
    role: "Principal",
    company: "EduWorld School",
    rating: 5,
  },
  {
    quote: "Support is exceptional. Any issue we've had was resolved within hours. The platform just works, and when we need help, they're there.",
    author: "David Kim",
    role: "IT Director",
    company: "RetailMax",
    rating: 5,
  },
  {
    quote: "Multi-currency and multi-company features made expanding internationally seamless. We're now in 5 countries with one ERP.",
    author: "Anna Schmidt",
    role: "CFO",
    company: "Acme Corporation",
    rating: 5,
  },
]

export function TestimonialsSection() {
  return (
    <section id="testimonials" className="py-24 bg-muted/30">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        {/* Section Header */}
        <div className="text-center mb-16">
          <Badge variant="outline" className="mb-4">Testimonials</Badge>
          <h2 className="text-4xl md:text-5xl font-bold mb-6">
            Loved by Businesses Worldwide
          </h2>
          <p className="text-xl text-muted-foreground max-w-2xl mx-auto">
            Join thousands of organizations that trust ERPNext SaaS 
            for their business operations.
          </p>
        </div>

        {/* Testimonials Grid */}
        <div className="grid gap-6 md:grid-cols-2 lg:grid-cols-3">
          {testimonials.map((testimonial, index) => (
            <Card key={index} className="bg-card">
              <CardContent className="pt-6">
                {/* Rating */}
                <div className="flex gap-1 mb-4">
                  {Array.from({ length: testimonial.rating }).map((_, i) => (
                    <Star key={i} className="h-5 w-5 fill-yellow-500 text-yellow-500" />
                  ))}
                </div>

                {/* Quote */}
                <blockquote className="text-lg mb-6">
                  "{testimonial.quote}"
                </blockquote>

                {/* Author */}
                <div className="flex items-center gap-4">
                  <div className="h-12 w-12 rounded-full bg-primary/10 flex items-center justify-center text-primary font-semibold">
                    {testimonial.author.split(' ').map(n => n[0]).join('')}
                  </div>
                  <div>
                    <div className="font-semibold">{testimonial.author}</div>
                    <div className="text-sm text-muted-foreground">
                      {testimonial.role}, {testimonial.company}
                    </div>
                  </div>
                </div>
              </CardContent>
            </Card>
          ))}
        </div>
      </div>
    </section>
  )
}
